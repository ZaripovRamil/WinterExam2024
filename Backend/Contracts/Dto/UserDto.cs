namespace Contracts.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public int Rating { get; set; }
}