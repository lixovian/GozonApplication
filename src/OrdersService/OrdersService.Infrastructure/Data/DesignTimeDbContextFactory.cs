using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrdersService.Infrastructure.Data;

internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OrdersDbContext>
{
    public OrdersDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseNpgsql("Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;")
            .Options;

        return new OrdersDbContext(options);
    }
}