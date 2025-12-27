using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrdersService.Infrastructure.Data;
using OrdersService.Infrastructure.Data.Orders;
using OrdersService.Infrastructure.ExternalServices.PaymentsService;
using OrdersService.Infrastructure.Jobs;
using OrdersService.Infrastructure.Kafka;
using OrdersService.Infrastructure.Kafka.Dtos;
using OrdersService.UseCases.Orders.AddOrder;
using OrdersService.UseCases.Orders.ApplyPaymentResult;
using OrdersService.UseCases.Orders.GetOrderStatus;
using OrdersService.UseCases.Orders.ListOrders;

namespace OrdersService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // --- DB ---
        services.AddDbContext<OrdersDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        // --- Repositories (Orders) ---
        services.AddScoped<IAddOrderRepository, AddOrderRepository>();
        services.AddScoped<IListOrdersRepository, ListOrdersRepository>();
        services.AddScoped<IGetOrderStatusRepository, GetOrderStatusRepository>();

        // Transactional Inbox (payment results -> orders)
        services.AddScoped<IApplyPaymentResultRepository, ApplyPaymentResultRepository>();

        // --- Hosted services ---
        services.AddHostedService<MigrationRunner>();

        // === Outbox Publisher -> Kafka producer (PaymentRequested) ===
        services.AddOptions<OrdersProducerOptions>()
            .BindConfiguration("OrdersProducer")
            .ValidateOnStart();

        services.AddScoped<IProducer<Null, PaymentRequestedDto>>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<OrdersProducerOptions>>();

            var config = new ProducerConfig
            {
                BootstrapServers = opts.Value.BootstrapServers
            };

            return new ProducerBuilder<Null, PaymentRequestedDto>(config)
                .SetValueSerializer(new JsonValueSerializer<PaymentRequestedDto>())
                .Build();
        });

        services.AddHostedService<OutboxPublisherBackgroundService>();

        // === Payments Results Consumer (manual commit) ===
        services.AddOptions<OrdersPaymentsResultsConsumerOptions>()
            .BindConfiguration("PaymentsResultsConsumer")
            .ValidateOnStart();

        services.AddTransient<IConsumer<Ignore, PaymentResultDto?>>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<OrdersPaymentsResultsConsumerOptions>>();

            var config = new ConsumerConfig
            {
                BootstrapServers = opts.Value.BootstrapServers,
                GroupId = opts.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false // как в занятии (commit только после обработки)
            };

            return new ConsumerBuilder<Ignore, PaymentResultDto?>(config)
                .SetValueDeserializer(new JsonValueDeserializer<PaymentResultDto>())
                .Build();
        });

        services.AddHostedService<PaymentsResultsConsumer>();

        return services;
    }
}
