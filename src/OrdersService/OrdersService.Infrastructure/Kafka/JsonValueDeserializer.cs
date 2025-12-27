using System.Text.Json;
using Confluent.Kafka;

namespace OrdersService.Infrastructure.Kafka;

internal sealed class JsonValueDeserializer<T> : IDeserializer<T?>
{
    public T? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull) return default;
        return JsonSerializer.Deserialize<T?>(data);
    }
}