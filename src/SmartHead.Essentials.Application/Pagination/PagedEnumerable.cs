using System;
using System.Collections.Generic;

namespace SmartHead.Essentials.Application.Pagination
{
    public class PagedEnumerable<T>
    {
        public PagedEnumerable(IEnumerable<T> items, PaginationModel pagination)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            Pagination = pagination ?? throw new ArgumentNullException(nameof(pagination));
        }

        public PaginationModel Pagination { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
