using AutoMapper;
using Contracts.Dbo;
using Contracts.Dto;
using DatabaseServices.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models;
using WinterExam24.Features.Moves;

namespace WinterExam24.Hubs;

[Authorize]
public class MainHub : Hub
{
    public MainHub(IRoomRepository roomRepository, IMapper mapper, IUserRepository users, IGameResultCalculator gameResultCalculator, IBus bus)
    {
        _rooms = roomRepository;
        _mapper = mapper;
        _users = users;
        _gameResultCalculator = gameResultCalculator;
        _bus = bus;
    }

    private readonly IUserRepository _users;
    private readonly IRoomRepository _rooms;
    private readonly IMapper _mapper;
    private readonly IGameResultCalculator _gameResultCalculator;
    private readonly IBus _bus;
    public async Task Enter(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var roomId = Guid.Parse(groupName);
        var room = await _rooms.GetAsync(roomId);
        if(room is null)
            await Clients.Client(Context.ConnectionId).SendAsync("Receive", new RoomDto {StatusCode = 404});
        else
            await Clients.Group(groupName).SendAsync("Receive", _mapper.Map<Room, RoomDto>(room));
    }

    public async Task JoinToGame(RoomDto roomDto, string groupName)
    {
        var roomId = Guid.Parse(groupName);
        var room = await _rooms.GetAsync(roomId);
        var user = await _users.FindByClaimAsync(Context.User!);
        if(room is null)
            await Clients.Client(Context.ConnectionId).SendAsync("Receive", new RoomDto {StatusCode = 404});
        else if (room.Players.Count == 2)
            await Clients.Client(Context.ConnectionId).SendAsync("Receive", new RoomDto {StatusCode = 400});
        else
        {
            room.Players.Add(user!);
            await _rooms.UpdatePlayers(room.Id,room.Players);
            await Clients.Group(groupName).SendAsync("Receive", _mapper.Map<Room, RoomDto>(room));
        }
    }
    
    public async Task SendChatMessage(string message, string groupName) {
        var user = await _users.FindByClaimAsync(Context.User!);
        await Clients.Group(groupName).SendAsync("ReceiveChatMessage", message, user.UserName);
    }

    public async Task Move(int move, string groupName)
    {
        var roomId = Guid.Parse(groupName);
        var room = await _rooms.GetAsync(roomId);
        if (room is null)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("Receive", new RoomDto {StatusCode = 404});
            return;
        }
            
        var user = await _users.FindByClaimAsync(Context.User!);
        if (room.Players.All(u => u.Id != user!.Id))
        {
            await Clients.Client(Context.ConnectionId).SendAsync("Receive", new RoomDto {StatusCode = 403});
            return;
        }
        var parsedMove = (Common.Move)move;
        room.GameState.Moves.Add(user!.UserName!, parsedMove);
        await _rooms.UpdateGameState(room.Id, room.GameState);
        if (room.GameState.Moves.Count == 2)
        {
            _gameResultCalculator.UpdateRoomWithGameResult(room);
            if(room.GameState.WinnerUsername == "")
                foreach (var player in room.Players)
                {
                    await _bus.Publish(new UserRatingDbo() { UserId = player.Id, Rating = player.Rating + 1 });
                }
            else 
                foreach (var player in room.Players)
                {
                    if(player.UserName == room.GameState.WinnerUsername)
                        await _bus.Publish(new UserRatingDbo { UserId = player.Id, Rating = player.Rating + 3 });
                    else await _bus.Publish(new UserRatingDbo { UserId = player.Id, Rating = player.Rating + - 1 });
                }
            await Clients.Client(Context.ConnectionId).SendAsync("Receive",  _mapper.Map<Room,RoomDto>(room));
            //await Clients.Group(groupName).SendAsync("ReceiveChatMessage", _mapper.Map<Room,RoomDto>(room));
            room.GameState = new GameState();
            await _rooms.UpdateGameState(room.Id, room.GameState);
        }
            
    }
}