using System.Text.Json;
using AutoMapper;
using Contracts.Dbo;
using Contracts.Dto;
using Database;
using Models;
using MongoDB.Driver;
using Utils.CQRS;
using Utils.WebApplicationExtensions;
using WinterExam24.ConfigurationExtensions;
using WinterExam24.Features.Auth.SignUp;
using WinterExam24.Features.Moves;
using WinterExam24.Features.Rating.Get;
using WinterExam24.Hubs;
using WinterExam24.ServiceCollectionExtensions;
using IMongoDatabase = Database.IMongoDatabase;
using ResultDto = WinterExam24.Features.Auth.SignUp.ResultDto;

var builder = WebApplication.CreateBuilder(args);

AddEnvironmentFilesExtension.AddEnvironmentFiles();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddSignalR();   
var config = new MapperConfiguration(cfg
    =>
{
    cfg.CreateMap<User, UserDbo>().ReverseMap();
    cfg.CreateMap<User, UserDto>();
    cfg.CreateMap<GameState, GameStateDto>();
    cfg.CreateMap<Room, RoomDto>();
    cfg.CreateMap<Room, RoomDbo>()
        .ForMember(dbo => dbo.GameState, 
            opt => 
                opt.MapFrom(room => JsonSerializer.SerializeToDocument(room.GameState, new JsonSerializerOptions())));
    cfg.CreateMap<RoomDbo, Room>()
        .ForMember(room => room.GameState,
            opt =>
                opt.MapFrom(dbo => dbo.GameState.Deserialize<GameState>(
                    new JsonSerializerOptions {PropertyNameCaseInsensitive = true})));
});
builder.Services.AddScoped<IGameResultCalculator, GameResultCalculator>();
builder.Services.AddSingleton<IMongoClient>(x => new MongoClient("mongodb://host.docker.internal:27017"));
builder.Services.AddSingleton<IMongoDatabase, MongoDatabase>();
builder.Services.AddSingleton(config.CreateMapper());
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services
    .AddScoped<
        IQueryHandler<WinterExam24.Features.Rooms.GetList.Query, WinterExam24.Features.Rooms.GetList.ResultDto>, 
    WinterExam24.Features.Rooms.GetList.QueryHandler>();
builder.Services
        .AddScoped<
    ICommandHandler<WinterExam24.Features.Rooms.Create.Command, WinterExam24.Features.Rooms.Create.ResultDto>,
    WinterExam24.Features.Rooms.Create.CommandHandler>();

builder.Services.AddScoped<IQueryHandler<Query, WinterExam24.Features.Rating.Get.ResultDto>, QueryHandler>();
builder.Services
    .AddScoped<ICommandHandler<WinterExam24.Features.Auth.SignIn.Command, WinterExam24.Features.Auth.SignIn.ResultDto>,
        WinterExam24.Features.Auth.SignIn.CommandHandler>();
builder.Services.AddScoped<ICommandHandler<Command, ResultDto>, CommandHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();


app.UseCors();

app.UseAuthorization();

app.MapControllers();
app.MapHub<MainHub>("/room");
app.Run();