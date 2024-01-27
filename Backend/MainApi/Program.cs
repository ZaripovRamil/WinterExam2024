using System.Text.Json;
using AutoMapper;
using Contracts.Dbo;
using Contracts.Dto;
using Database;
using MassTransit;
using Models;
using MongoDB.Driver;
using Utils.WebApplicationExtensions;
using WinterExam24.ConfigurationExtensions;
using WinterExam24.Features.Moves;
using WinterExam24.Hubs;
using WinterExam24.ServiceCollectionExtensions;
using WinterExam24.Services;
using IMongoDatabase = Database.IMongoDatabase;

var builder = WebApplication.CreateBuilder(args);

AddEnvironmentFilesExtension.AddEnvironmentFiles();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSignalR();
builder.Services.AddMassTransit(c =>
{
    c.SetKebabCaseEndpointNameFormatter();
    var assembly = typeof(Program).Assembly;
    c.AddConsumers(assembly);
    c.UsingRabbitMq((ctx, cfg) => { cfg.ConfigureEndpoints(ctx); });
});
var config = new MapperConfiguration(cfg
    =>
{
    cfg.CreateMap<User, UserDbo>().ReverseMap();
    cfg.CreateMap<User, UserDto>();
    cfg.CreateMap<GameState, GameStateDto>();
    cfg.CreateMap<Room, RoomDto>()
        .ForMember(r => r.OwnerName, opt =>
            opt.MapFrom(r => r.Players.Count > 0 ? r.Players[0].UserName : ""));
    cfg.CreateMap<Room, RoomDbo>()
        .ForMember(dbo => dbo.GameState,
            opt =>
                opt.MapFrom(room => JsonSerializer.SerializeToDocument(room.GameState, new JsonSerializerOptions())));
    cfg.CreateMap<RoomDbo, Room>()
        .ForMember(room => room.GameState,
            opt =>
                opt.MapFrom(dbo => dbo.GameState.Deserialize<GameState>(
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })));
});
builder.Services.AddScoped<IGameResultCalculator, GameResultCalculator>();
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient("mongodb://host.docker.internal:27017"));
builder.Services.AddSingleton<IMongoDatabase, MongoDatabase>();
builder.Services.AddSingleton(config.CreateMapper());
builder.Services.AddApplicationServices(builder.Configuration);
// builder.Services.AddHostedService<RoomCleaner>();
builder.Services.AddRequestHandlers();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();


app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<MainHub>("/room");
app.Run();