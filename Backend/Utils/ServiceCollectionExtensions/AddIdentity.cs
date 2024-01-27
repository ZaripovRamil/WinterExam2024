using Contracts.Dbo;
using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Utils.ServiceCollectionExtensions;

public static class AddIdentityExtension
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, bool isDevelopment)
    {
        services.AddIdentity<UserDbo, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 0;
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
// using Contracts.Dbo;
// using Database;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace Utils.ServiceCollectionExtensions;
//
// public static class AddIdentityExtension
// {
//     public static IServiceCollection AddIdentity(this IServiceCollection services, bool isDevelopment)
//     {
//         services.AddIdentityCore<UserDbo>(options =>
//             {
//                 options.Password.RequireDigit = false;
//                 options.Password.RequiredLength = 0;
//                 options.Password.RequireNonAlphanumeric = false;
//                 options.Password.RequireUppercase = false;
//                 options.Password.RequireLowercase = false;
//                 options.SignIn.RequireConfirmedPhoneNumber = false;
//                 options.SignIn.RequireConfirmedEmail = false;
//                 options.SignIn.RequireConfirmedAccount = false;
//             })
//             .AddRoles<IdentityRole<Guid>>()
//             .AddUserManager<UserDbo>()
//             .AddSignInManager<UserDbo>()
//             .AddEntityFrameworkStores<AppDbContext>()
//             .AddDefaultTokenProviders();
//
//         return services;
//     }
// }