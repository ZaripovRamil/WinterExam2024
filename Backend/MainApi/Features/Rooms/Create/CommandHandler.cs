using AutoMapper;
using DatabaseServices.Repositories;
using Models;
using Utils.CQRS;

namespace WinterExam24.Features.Rooms.Create;

public class CommandHandler : ICommandHandler<Command, ResultDto>
{
    private readonly IUserRepository _users;
    private readonly IRoomRepository _rooms;
    private readonly IMapper _mapper;

    public CommandHandler(IRoomRepository rooms, IMapper mapper, IUserRepository users)
    {
        _rooms = rooms;
        _mapper = mapper;
        _users = users;
    }

    public async Task<Result<ResultDto>> Handle(Command request, CancellationToken cancellationToken)
    {
        
        var room = new Room() { Players = new List<User> { (await _users.GetAsync(request.CreatorId))! }, 
            Id = Guid.NewGuid(), 
            GameState = new GameState(),
            Created = DateTime.Now
        };
        await _rooms.AddAsync(room);
        
        return new Result<ResultDto>(new ResultDto() { GameId = room.Id });
    }
}