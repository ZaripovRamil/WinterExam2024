using AutoMapper;
using Contracts.Dto;
using DatabaseServices.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models;
using WinterExam24.Features.Moves;

namespace WinterExam24.Hubs;

[Authorize]
public class MainHub : Hub
{
    public MainHub(IRoomRepository roomRepository, IMapper mapper, IUserRepository users, IGameResultCalculator gameResultCalculator)
    {
        _rooms = roomRepository;
        _mapper = mapper;
        _users = users;
        _gameResultCalculator = gameResultCalculator;
    }

    private readonly IUserRepository _users;
    private readonly IRoomRepository _rooms;
    private readonly IMapper _mapper;
    private IGameResultCalculator _gameResultCalculator;
    public async Task Enter(string username, string groupName)
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
    
    public async Task SendChatMessage(string message, string userName, string groupName) {
        await Clients.Group(groupName).SendAsync("ReceiveChatMessage", message, userName);
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
        if (room.GameState.Moves.Count == 2)
        {
            _gameResultCalculator.UpdateRoomWithGameResult(room);
            await Clients.Group(groupName).SendAsync("ReceiveChatMessage", _mapper.Map<Room,RoomDto>(room));
        }
            
    }
}