using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Utils.ServiceCollectionExtensions;

public static class AddDbContextExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql("Server=localhost;Database=someDatabase;Port=5432;username=postgres;SSLMode=Prefer",
                b => b.MigrationsAssembly("Database")));
        
        return services;
    }
}