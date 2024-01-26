using Microsoft.AspNetCore.Mvc;
using Utils.CQRS;
using WinterExam24.Features.Auth.SignIn;
using InputDto = WinterExam24.Features.Auth.SignUp.InputDto;

namespace WinterExam24.Controllers;

[ApiController]
[Route("[action]")]
public class AuthController : Controller
{
    private readonly ICommandHandler<Command, ResultDto> _signInHandler;
    private readonly ICommandHandler<Features.Auth.SignUp.Command, Features.Auth.SignUp.ResultDto> _signUpHandler;

    public AuthController(ICommandHandler<Command, ResultDto> signInHandler, ICommandHandler<Features.Auth.SignUp.Command, Features.Auth.SignUp.ResultDto> signUpHandler)
    {
        _signInHandler = signInHandler;
        _signUpHandler = signUpHandler;
    }
    
    [HttpPost]
    public async Task<IActionResult> SignIn(Features.Auth.SignIn.InputDto loginData)
    {
        var res = await _signInHandler.Handle(new Command(loginData), new CancellationToken(false));
        return res.IsSuccessful ? Ok(res.Value) : BadRequest(res.Value);
    }
    
    [HttpPost]
    public async Task<IActionResult> SignUp(InputDto registrationData)
    {
        var res = await _signUpHandler.Handle(new Features.Auth.SignUp.Command(registrationData), new CancellationToken(false));
        return res.IsSuccessful ? Ok(res.Value) : BadRequest(res.Value);
    }
}