using Microsoft.AspNetCore.Identity;

namespace Models;

public class User : IdentityUser<Guid>
{
    public int Rating { get; set; }
}