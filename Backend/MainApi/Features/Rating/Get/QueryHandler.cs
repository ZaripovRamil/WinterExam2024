using AutoMapper;
using Contracts.Dto;
using DatabaseServices.Repositories;
using Models;
using Utils.CQRS;

namespace WinterExam24.Features.Rating.Get;

public class QueryHandler : IQueryHandler<Query, ResultDto>
{
    private readonly IUserRepository _users;
    private readonly IMapper _mapper;

    public QueryHandler(IUserRepository users, IMapper mapper)
    {
        _users = users;
        _mapper = mapper;
    }

    public async Task<Result<ResultDto>> Handle(Query request, CancellationToken cancellationToken)
    {
        return new Result<ResultDto>(
            new ResultDto
            {
                Players =_mapper.Map<User[], UserDto[]>((await _users.GetAll()).OrderByDescending(user => user.Rating).ToArray())
            });
    }
}