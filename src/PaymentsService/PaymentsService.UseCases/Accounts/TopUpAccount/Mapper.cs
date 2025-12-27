using PaymentsService.Entities.Models;
using Riok.Mapperly.Abstractions;

namespace PaymentsService.UseCases.Accounts.TopUpAccount;

[Mapper]
internal static partial class Mapper
{
    internal static TopUpAccountResponse ToDto(this AccountTransaction tx, Account account) =>
        new TopUpAccountResponse(
            AccountId: account.Id,
            UserId: account.UserId,
            Amount: tx.Amount,
            Key: tx.Key,
            CreatedAt: tx.CreatedAt
        );
}