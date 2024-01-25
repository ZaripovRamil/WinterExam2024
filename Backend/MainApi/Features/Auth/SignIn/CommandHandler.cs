using DatabaseServices.Repositories;
using Utils.CQRS;

namespace WinterExam24.Features.Auth.SignIn;

public class CommandHandler : ICommandHandler<Command, ResultDto>
{
    private readonly IUserRepository _users;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    public CommandHandler(IUserRepository users, IJwtTokenGenerator jwtTokenGenerator)
    {
        _users = users;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<ResultDto>> Handle(Command request, CancellationToken cancellationToken)
    {
        var (username, password) = (request.LoginData.Username, request.LoginData.Password);
        var signInResult = await _users.SignInAsync(username, password);
        if (signInResult.Succeeded)
        {
            var userId = (await _users.FindByNameAsync(username))!.Id;
            var token = await _jwtTokenGenerator.GenerateJwtTokenAsync(username);
            if (token != null) return new Result<ResultDto>(new ResultDto() { IsSuccessful = true, Token = token, UserId = userId});
        }

        return new Result<ResultDto>(new ResultDto() { IsSuccessful = false, Token = "", UserId = new Guid() });
    }
}