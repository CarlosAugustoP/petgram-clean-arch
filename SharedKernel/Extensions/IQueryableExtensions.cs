using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;

namespace SharedKernel.Extensions
{
    public static class IQueryableExtensions
    {
        public static PaginatedList<T> Paginate<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex < 1) throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index must be greater than or equal to 1.");
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1.");

            var totalCount = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, totalCount, pageIndex, pageSize);
        }

        public static async Task<PaginatedList<T>> PaginateAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageIndex < 1) throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index must be greater than or equal to 1.");
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1.");

            var totalCount = await source.CountAsync(cancellationToken);
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, totalCount, pageIndex, pageSize);
        }
    }
}