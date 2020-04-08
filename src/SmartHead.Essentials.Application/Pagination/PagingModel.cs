using System;

namespace SmartHead.Essentials.Application.Pagination
{
    public class PagingModel
    {
        public PagingModel(int count, int size, int page = 1)
        {
            Page = page;
            Total = (int) Math.Ceiling(count / (double) size);
            Size = size;
            ItemsTotal = count;
        }

        public int ItemsTotal { get; }
        public int Page { get; }
        public int Total { get; }
        public int Size { get; }
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < Total;
    }
}