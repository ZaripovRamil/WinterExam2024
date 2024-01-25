using DatabaseServices.Repositories;
using Models;
using Utils.CQRS;

namespace WinterExam24.Features.Auth.SignUp;

public class CommandHandler : ICommandHandler<Command, ResultDto>
{
    private readonly IUserRepository _users;

    public CommandHandler(IUserRepository users)
    {
        _users = users;
    }


    public async Task<Result<ResultDto>> Handle(Command request, CancellationToken cancellationToken)
    {
        var (username, password) = (request.RegistrationData.Username, request.RegistrationData.Password);
        var regResult = await _users.CreateAsync(new User(){UserName = username}, password);
        if (!regResult.Succeeded)
            return new Result<ResultDto>(new ResultDto() { IsSuccessful = false, UserId = new Guid() });
        var user = await _users.FindByNameAsync(username);
        return new Result<ResultDto>(new ResultDto(){IsSuccessful = true, UserId = user!.Id});
    }
}