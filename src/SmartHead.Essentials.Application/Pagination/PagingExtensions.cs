using System.Linq;

namespace SmartHead.Essentials.Application.Pagination
{
    public static class PagingExtensions
    {
        public static IQueryable<T> PaginateQueryable<T>(this IQueryable<T> queryable, int page, int size)
            => queryable
                .Skip((page - 1) * size)
                .Take(size);

        public static PagedResponse<TModel> Paginate<TModel>(
            this IQueryable<TModel> query,
            int page = 1,
            int size = 10)
        {
            if (page < 1) page = 1;
            if (size < 1) size = 10;

            var paginatedQueryable = query
                .PaginateQueryable(page, size);

            var paginationModel = new PagingModel(query.Count(), size, page);

            return new PagedResponse<TModel>(paginatedQueryable, paginationModel);
        }
    }
}