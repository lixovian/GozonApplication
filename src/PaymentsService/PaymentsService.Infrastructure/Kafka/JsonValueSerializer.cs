using System.Text.Json;
using Confluent.Kafka;

namespace PaymentsService.Infrastructure.Kafka;

internal sealed class JsonValueSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
        => JsonSerializer.SerializeToUtf8Bytes(data);
}