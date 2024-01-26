using Utils.CQRS;

namespace WinterExam24.Features.Rooms.Create;

public class Command : ICommand<ResultDto>
{
    public Guid CreatorId { get; set; }
    public int MaxRating { get; set; }
}