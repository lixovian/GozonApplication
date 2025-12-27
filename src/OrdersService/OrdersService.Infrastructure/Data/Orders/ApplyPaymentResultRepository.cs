using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OrdersService.Infrastructure.Data.Dtos;
using OrdersService.UseCases.Orders.ApplyPaymentResult;

namespace OrdersService.Infrastructure.Data.Orders;

internal sealed class ApplyPaymentResultRepository(OrdersDbContext dbContext) : IApplyPaymentResultRepository
{
    public void ApplyPaymentResult(
        Guid orderId,
        string key,
        bool isSuccess,
        string? payloadJson,
        string messageType)
    {
        using var tx = dbContext.Database.BeginTransaction();

        var alreadyProcessed = dbContext.Inbox.Any(x => x.Key == key);
        if (alreadyProcessed)
        {
            tx.Commit();
            return;
        }

        var inbox = new InboxMessageDto(Guid.NewGuid())
        {
            Type = messageType,
            Key = key,
            Payload = payloadJson ?? string.Empty,
            ReceivedAt = DateTimeOffset.UtcNow
        };

        dbContext.Add(inbox);

        var newStatus = isSuccess ? "FINISHED" : "CANCELLED";

        var updated = dbContext.Orders
            .Where(o => o.Id == orderId)
            .Where(o => o.Status != newStatus)
            .ExecuteUpdate(s => s.SetProperty(o => o.Status, newStatus));

        dbContext.SaveChanges();
        tx.Commit();
    }
}