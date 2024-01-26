using Microsoft.AspNetCore.Mvc;
using Utils.CQRS;
using WinterExam24.Features.Rating.Get;

namespace WinterExam24.Controllers;


[ApiController]
[Route("[controller]")]
public class RatingController: Controller
{
    private readonly IQueryHandler<Query, ResultDto> _getRatingHandler;

    public RatingController(IQueryHandler<Query, ResultDto> getRatingHandler)
    {
        _getRatingHandler = getRatingHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetRating()
    {
        return Ok((await _getRatingHandler.Handle(new Query(), new CancellationToken())).Value!.Players);
    }
}