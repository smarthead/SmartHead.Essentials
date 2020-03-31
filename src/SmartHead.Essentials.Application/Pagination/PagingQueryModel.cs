namespace SmartHead.Essentials.Application.Pagination
{
    public class PagingQueryModel
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}