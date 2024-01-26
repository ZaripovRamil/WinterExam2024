using Contracts.Dbo.Joint;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Contracts.Dbo;

[PrimaryKey("Id")]
public class UserDbo : IdentityUser<Guid>
{
    public List<RoomDbo> Rooms { get; set; } = new();
    public List<UserRoomDbo> UserRooms { get; set; } = new();
}