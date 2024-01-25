using AutoMapper;
using Contracts.Dbo;
using DatabaseServices.Repositories;
using Models;
using Utils.CQRS;
using Utils.WebApplicationExtensions;
using WinterExam24.ConfigurationExtensions;
using WinterExam24.Features.Auth.SignUp;
using WinterExam24.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

AddEnvironmentFilesExtension.AddEnvironmentFiles();
builder.Configuration.AddEnvironmentVariables();
var config = new MapperConfiguration(cfg
    =>
{
    cfg.CreateMap<User, UserDbo>().ReverseMap();
});
builder.Services.AddSingleton(config.CreateMapper());
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddScoped<ICommandHandler<WinterExam24.Features.Auth.SignIn.Command, WinterExam24.Features.Auth.SignIn.ResultDto>, WinterExam24.Features.Auth.SignIn.CommandHandler>();
builder.Services.AddScoped<ICommandHandler<Command, ResultDto>, CommandHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();