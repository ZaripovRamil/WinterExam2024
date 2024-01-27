using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Models.Configuration;
using Utils.ServiceCollectionExtensions;
using WinterExam24.Features.Auth;

namespace WinterExam24.ServiceCollectionExtensions;

public static class AddApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.Configure<JwtTokenSettings>(configuration.GetSection("JWTTokenSettings"));

        services.AddIdentity(true);
        services.AddRepositories(configuration);
        
        services.AddJwtAuthorization(configuration);
        services.AddSwaggerWithAuthorization();
        services.AddAllCors();
        
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });
        return services;
    }
}