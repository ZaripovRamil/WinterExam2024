namespace WinterExam24.Features.Auth.SignIn;

public class ResultDto
{
    public bool IsSuccessful { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
}