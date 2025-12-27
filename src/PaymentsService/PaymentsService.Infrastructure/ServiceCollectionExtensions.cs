using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaymentsService.Infrastructure.Data;
using PaymentsService.Infrastructure.Kafka;
using PaymentsService.Infrastructure.Kafka.Dtos;
using PaymentsService.UseCases.Accounts.AddAccount;
using PaymentsService.UseCases.Accounts.GetBalance;
using PaymentsService.UseCases.Accounts.TopUpAccount;
using PaymentsService.UseCases.Payments.ProcessPayment;

namespace PaymentsService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>((serviceProvider, builder) =>
        {
            var connectionString = serviceProvider.GetRequiredService<IConfiguration>()
                .GetConnectionString("Default");

            builder.UseNpgsql(connectionString);
        });

        // Repositories
        services
            .AddScoped<IAddAccountRepository, PaymentsRepository>()
            .AddScoped<ITopUpAccountRepository, PaymentsRepository>()
            .AddScoped<IGetBalanceRepository, PaymentsRepository>()
            .AddScoped<IProcessPaymentRepository, PaymentsRepository>();

        services.AddHostedService<MigrationRunner>();

        // Consumer: PaymentRequested
        services.AddOptions<PaymentsConsumerOptions>()
            .BindConfiguration("PaymentsConsumer")
            .ValidateOnStart();

        services.AddTransient<IConsumer<Ignore, PaymentRequestedDto?>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<PaymentsConsumerOptions>>();

            var config = new ConsumerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,
                GroupId = options.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            return new ConsumerBuilder<Ignore, PaymentRequestedDto?>(config)
                .SetValueDeserializer(new JsonValueDeserializer<PaymentRequestedDto>())
                .Build();
        });

        services.AddHostedService<PaymentRequestedConsumer>();

        // Producer: PaymentResult (outbox -> topic)
        services.AddOptions<PaymentsProducerOptions>()
            .BindConfiguration("PaymentsProducer")
            .ValidateOnStart();

        services.AddScoped<IProducer<Null, PaymentResultDto>>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<PaymentsProducerOptions>>();

            var config = new ProducerConfig
            {
                BootstrapServers = opts.Value.BootstrapServers
            };

            return new ProducerBuilder<Null, PaymentResultDto>(config)
                .SetValueSerializer(new JsonValueSerializer<PaymentResultDto>())
                .Build();
        });

        services.AddHostedService<PaymentsOutboxPublisher>();

        return services;
    }
}
