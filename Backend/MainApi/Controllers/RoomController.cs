using DatabaseServices.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils.CQRS;
using WinterExam24.Features.Rooms.Create;
using WinterExam24.Features.Rooms.GetList;
using ResultDto = WinterExam24.Features.Rooms.GetList.ResultDto;

namespace WinterExam24.Controllers;

[Authorize]
[ApiController]
[Route("games")]
public class RoomController : Controller
{
    private readonly IQueryHandler<Query, ResultDto> _getRoomsHandler;
    private readonly ICommandHandler<Command, WinterExam24.Features.Rooms.Create.ResultDto> _roomCreationHandler;
    private readonly IUserRepository _userRepository;

    public RoomController(
        IQueryHandler<Query, ResultDto> getRoomsHandler, 
        ICommandHandler<Command, Features.Rooms.Create.ResultDto> roomCreationHandler, IUserRepository userRepository)
    {
        _getRoomsHandler = getRoomsHandler;
        _roomCreationHandler = roomCreationHandler;
        _userRepository = userRepository;
    }
    [HttpGet]
    public async Task<IActionResult> GetRooms([FromQuery] int limit = 10, [FromQuery] int page = 1)
    {
        var queryResult = await _getRoomsHandler.Handle(new Query() { Page = page, Size = limit },
            new CancellationToken());
        return queryResult.IsSuccessful ? Ok(queryResult.Value.Rooms) : BadRequest(queryResult.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] InputDto roomCreationData)
    {
        var user = await _userRepository.FindByClaimAsync(User);
        if (user is null) return Unauthorized();
        var commandResult = await _roomCreationHandler.Handle(new Command()
        {
            MaxRating = roomCreationData.MaxRating,
            CreatorId = user.Id
        }, new CancellationToken());
        return Ok(commandResult.Value);
    }
}