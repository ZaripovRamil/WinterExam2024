using Utils.CQRS;

namespace WinterExam24.Features.Auth.SignUp;

public record Command(InputDto RegistrationData) : ICommand<ResultDto>;