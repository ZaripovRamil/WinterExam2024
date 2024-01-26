using Utils.CQRS;

namespace WinterExam24.Features.Rooms.GetList;

public class Query : IQuery<ResultDto>
{
    public int Page { get; set; }
    public int Size { get; set; }
}