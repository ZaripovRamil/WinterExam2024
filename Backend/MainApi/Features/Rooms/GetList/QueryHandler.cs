using AutoMapper;
using Contracts.Dto;
using DatabaseServices.Repositories;
using Models;
using Utils.CQRS;

namespace WinterExam24.Features.Rooms.GetList;

public class QueryHandler : IQueryHandler<Query, ResultDto>
{
    private readonly IRoomRepository _rooms;
    private readonly IMapper _mapper;

    public QueryHandler(IRoomRepository rooms, IMapper mapper)
    {
        _rooms = rooms;
        _mapper = mapper;
    }

    public async Task<Result<ResultDto>> Handle(Query request, CancellationToken cancellationToken)
    {
        if (request.Size == 0 || request.Page <= 0)
            return new Result<ResultDto>("incorrect parameters");
        var rooms = (await _rooms.GetAll()).OrderBy(room => room.Players.Count)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToArray();
        if(rooms.Length == 0)return new Result<ResultDto>("incorrect parameters");
        return new Result<ResultDto>(new ResultDto
        {
            Rooms = _mapper.Map<Room[], RoomDto[]>(rooms)
        });
    }
}