using Utils.CQRS;

namespace WinterExam24.Features.Auth.SignIn;

public record  Command(InputDto LoginData) : ICommand<ResultDto>;