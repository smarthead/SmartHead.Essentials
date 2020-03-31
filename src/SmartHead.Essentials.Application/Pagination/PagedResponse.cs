using System;
using System.Linq;

namespace SmartHead.Essentials.Application.Pagination
{
    public class PagedResponse<T>
    {
        public PagedResponse(IQueryable<T> items, PagingModel pagination)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            Pagination = pagination ?? throw new ArgumentNullException(nameof(pagination));
        }

        public PagedResponse(IQueryable<T> items, int size, int page)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            if (page < 1) page = 1;
            if (size < 1) size = 10;

            Pagination = new PagingModel(items.Count(), size, page);
        }

        public PagingModel Pagination { get; set; }
        public IQueryable<T> Items { get; set; }
    }
}