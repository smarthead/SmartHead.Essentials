namespace SmartHead.Essentials.Application.Pagination
{
    public interface IPagingQueryModel
    {
        int Page { get; set; }
        int Size { get; set; }
    }
}