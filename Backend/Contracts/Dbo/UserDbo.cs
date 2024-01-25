using Contracts.Dbo.Joint;
using Microsoft.AspNetCore.Identity;

namespace Contracts.Dbo;

public class UserDbo : IdentityUser<Guid>
{
    public List<RoomDbo> Rooms { get; set; } = new();
    public List<UserRoomDbo> UserRooms { get; set; } = new();
}