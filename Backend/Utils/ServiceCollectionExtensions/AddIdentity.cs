using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace Utils.ServiceCollectionExtensions;

public static class AddIdentityExtension
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, bool isDevelopment)
    {
        services.AddIdentity<User, IdentityRole>(options =>
            {
                if (!isDevelopment) return;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}