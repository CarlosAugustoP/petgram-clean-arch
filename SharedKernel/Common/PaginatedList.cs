using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Common
{
    public class PaginatedList<T> : List<T>
    {
        public List<T> Items {get; set;}
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int Size { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            TotalCount = count;
            Size = pageSize;
            Items = items;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).
                ToListAsync(cancellationToken);
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}