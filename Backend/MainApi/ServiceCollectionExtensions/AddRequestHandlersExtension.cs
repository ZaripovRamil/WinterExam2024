using Utils.CQRS;


namespace WinterExam24.ServiceCollectionExtensions;

public static class AddRequestHandlersExtension
{
    public static void AddRequestHandlers(this IServiceCollection services)
    {
        services.AddScoped<
            IQueryHandler<Features.Rooms.GetList.Query, Features.Rooms.GetList.ResultDto>,
            Features.Rooms.GetList.QueryHandler>();
        services.AddScoped<
            ICommandHandler<Features.Rooms.Create.Command, Features.Rooms.Create.ResultDto>,
            Features.Rooms.Create.CommandHandler>();

        services.AddScoped<
            IQueryHandler<Features.Rating.Get.Query, Features.Rating.Get.ResultDto>,
            Features.Rating.Get.QueryHandler>();
        services.AddScoped<
            ICommandHandler<Features.Auth.SignIn.Command, Features.Auth.SignIn.ResultDto>,
            Features.Auth.SignIn.CommandHandler>();
        services.AddScoped<ICommandHandler<Features.Auth.SignUp.Command, Features.Auth.SignUp.ResultDto>,
            Features.Auth.SignUp.CommandHandler>();
    }
}