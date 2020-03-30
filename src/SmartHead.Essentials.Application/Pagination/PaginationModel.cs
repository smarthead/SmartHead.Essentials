using System;

namespace SmartHead.Essentials.Application.Pagination
{
    public class PaginationModel
    {
        public PaginationModel(int count, int size, int page = 1)
        {
            PageNumber = page;
            TotalPages = (int)Math.Ceiling(count / (double)size);
            PageSize = size;
        }

        public int PageNumber { get; }

        public int TotalPages { get; }

        public int PageSize { get; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}