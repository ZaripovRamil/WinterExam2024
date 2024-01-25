using Utils.ServiceCollectionExtensions;
using WinterExam24.Features.Auth;

namespace WinterExam24.ServiceCollectionExtensions;

public static class AddApplicationServicesExtensions
{
    public static void AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddIdentity(true);
        services.AddSwaggerWithAuthorization();
        services.AddAllCors();

        services.AddRepositories(configuration);
    }
}