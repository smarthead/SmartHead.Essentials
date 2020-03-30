using System.Linq;
using SmartHead.Essentials.Application.Pagination;

namespace SmartHead.Essentials.Extensions
{
    public static class EnumerableExtensions
    {
        public static PagedEnumerable<TModel> GetPage<TModel>(
            this IQueryable<TModel> query,
            int page = 1,
            int size = 10)
        {
            if (page < 1) page = 1;
            if (size < 1) size = 10;

            var paginatedQueryable = query
                .Skip((page - 1) * size)
                .Take(size);

            var paginationModel = new PaginationModel(query.Count(), size, page);

            return new PagedEnumerable<TModel>(paginatedQueryable, paginationModel);
        }
    }
}