using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PaymentsService.Entities.Models;
using PaymentsService.Infrastructure.Outbox.Dtos;
using PaymentsService.UseCases.Accounts.AddAccount;
using PaymentsService.UseCases.Accounts.GetBalance;
using PaymentsService.UseCases.Accounts.TopUpAccount;
using PaymentsService.UseCases.Payments.ProcessPayment;

namespace PaymentsService.Infrastructure.Data;

internal sealed class PaymentsRepository(AppDbContext dbContext) :
    IAddAccountRepository,
    ITopUpAccountRepository,
    IGetBalanceRepository,
    IProcessPaymentRepository
{
    public Account GetOrCreate(int userId, DateTimeOffset now)
    {
        var existing = dbContext.Accounts.SingleOrDefault(a => a.UserId == userId);
        if (existing is not null)
            return new Account(existing.Id, existing.UserId, existing.CreatedAt);

        var account = new Account(Guid.NewGuid(), userId, now);

        dbContext.Accounts.Add(new AccountDto(account.Id)
        {
            UserId = account.UserId,
            CreatedAt = account.CreatedAt
        });

        dbContext.SaveChanges();
        return account;
    }

    public Account? FindAccount(int userId)
    {
        var dto = dbContext.Accounts.SingleOrDefault(a => a.UserId == userId);
        return dto is null ? null : new Account(dto.Id, dto.UserId, dto.CreatedAt);
    }

    public AccountTransaction AddTopUpTransaction(AccountTransaction transaction)
    {
        // idempotency by Key
        var existing = dbContext.AccountTransactions.SingleOrDefault(t => t.Key == transaction.Key);
        if (existing is not null)
        {
            return new AccountTransaction(
                existing.Id,
                existing.AccountId,
                existing.Type == "DEBIT" ? TransactionType.Debit : TransactionType.TopUp,
                existing.Amount,
                existing.CreatedAt,
                existing.Key);
        }

        dbContext.AccountTransactions.Add(new AccountTransactionDto(transaction.Id)
        {
            AccountId = transaction.AccountId,
            Type = "TOPUP",
            Amount = transaction.Amount,
            CreatedAt = transaction.CreatedAt,
            Key = transaction.Key
        });

        dbContext.SaveChanges();
        return transaction;
    }

    // ----- GetBalance -----
    public Guid? FindAccountId(int userId)
    {
        return dbContext.Accounts
            .Where(a => a.UserId == userId)
            .Select(a => (Guid?)a.Id)
            .SingleOrDefault();
    }

    public decimal GetBalance(Guid accountId)
    {
        // ledger: sum signed amounts
        var sumTopUp = dbContext.AccountTransactions
            .Where(t => t.AccountId == accountId && t.Type == "TOPUP")
            .Sum(t => (decimal?)t.Amount) ?? 0;

        var sumDebit = dbContext.AccountTransactions
            .Where(t => t.AccountId == accountId && t.Type == "DEBIT")
            .Sum(t => (decimal?)t.Amount) ?? 0;

        return sumTopUp - sumDebit;
    }

    // ----- ProcessPayment (Transactional Inbox + exactly-once debit + Outbox result) -----
    public ProcessPaymentResponse Process(ProcessPaymentRequest request, DateTimeOffset now)
    {
        using var tx = dbContext.Database.BeginTransaction();

        // 1) Inbox dedupe by Key
        var existingInbox = dbContext.Inbox.SingleOrDefault(i => i.Key == request.Key);
        if (existingInbox is not null)
        {
            // Restore result from outbox (if any), otherwise infer by whether debit tx exists
            var existingOutbox = dbContext.Outbox
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault(o => o.Key == request.Key);

            if (existingOutbox is not null)
            {
                var dto = JsonSerializer.Deserialize<ProcessPaymentResponse>(existingOutbox.Payload);
                if (dto is not null)
                {
                    tx.Commit();
                    return dto;
                }
            }

            // fallback
            tx.Commit();
            return new ProcessPaymentResponse(request.OrderId, request.Key, "FAILED", "Already processed");
        }

        // store inbox first
        dbContext.Inbox.Add(new InboxMessage(Guid.NewGuid())
        {
            Type = "PaymentRequested",
            Key = request.Key,
            Payload = JsonSerializer.Serialize(request),
            ReceivedAt = now
        });

        // 2) Account exists?
        var account = dbContext.Accounts.SingleOrDefault(a => a.UserId == request.UserId);
        if (account is null)
        {
            var failed = new ProcessPaymentResponse(request.OrderId, request.Key, "FAILED", "ACCOUNT_NOT_FOUND");
            AddOutbox(failed, now, "PaymentFailed");
            dbContext.SaveChanges();
            tx.Commit();
            return failed;
        }

        // 3) Check balance
        var balance = GetBalance(account.Id);
        if (balance < request.Amount)
        {
            var failed = new ProcessPaymentResponse(request.OrderId, request.Key, "FAILED", "INSUFFICIENT_FUNDS");
            AddOutbox(failed, now, "PaymentFailed");
            dbContext.SaveChanges();
            tx.Commit();
            return failed;
        }

        // 4) Exactly-once debit: debit transaction with UNIQUE Key
        // If duplicate message arrives later, unique constraint prevents second debit.
        var debitTxId = Guid.NewGuid();

        // It is possible that two threads process same key concurrently:
        // unique index on AccountTransactions.Key will protect.
        dbContext.AccountTransactions.Add(new AccountTransactionDto(debitTxId)
        {
            AccountId = account.Id,
            Type = "DEBIT",
            Amount = request.Amount,
            CreatedAt = now,
            Key = request.Key
        });

        var succeeded = new ProcessPaymentResponse(request.OrderId, request.Key, "SUCCEEDED", null);
        AddOutbox(succeeded, now, "PaymentSucceeded");

        try
        {
            dbContext.SaveChanges();
            tx.Commit();
            return succeeded;
        }
        catch (DbUpdateException)
        {
            // If unique constraint hit (duplicate Key) – treat as already succeeded
            tx.Rollback();
            return new ProcessPaymentResponse(request.OrderId, request.Key, "SUCCEEDED", null);
        }

        void AddOutbox(ProcessPaymentResponse response, DateTimeOffset createdAt, string type)
        {
            dbContext.Outbox.Add(new OutboxMessage(Guid.NewGuid())
            {
                Type = type,
                Key = response.Key,
                Payload = JsonSerializer.Serialize(response),
                CreatedAt = createdAt,
                SentAt = null
            });
        }
    }
}
