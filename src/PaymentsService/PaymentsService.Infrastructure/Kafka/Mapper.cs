using PaymentsService.Infrastructure.Kafka.Dtos;
using PaymentsService.UseCases.Payments.ProcessPayment;
using Riok.Mapperly.Abstractions;

namespace PaymentsService.Infrastructure.Kafka;

[Mapper]
internal static partial class Mapper
{
    public static partial ProcessPaymentRequest ToRequest(this PaymentRequestedDto dto);
}