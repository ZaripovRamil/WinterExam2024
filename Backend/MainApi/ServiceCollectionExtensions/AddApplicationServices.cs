using Utils.ServiceCollectionExtensions;

namespace WinterExam24.ServiceCollectionExtensions;

public static class AddApplicationServicesExtensions
{
    public static void AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSwaggerWithAuthorization();
        services.AddAllCors();

        services.AddMediatorForAssembly(typeof(Program).Assembly);
        services.AddRepositories(configuration);
    }
}