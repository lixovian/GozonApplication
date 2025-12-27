using System.Text.Json;
using Confluent.Kafka;

namespace OrdersService.Infrastructure.ExternalServices.PaymentsService;

internal sealed class JsonValueSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
        => JsonSerializer.SerializeToUtf8Bytes(data);
}