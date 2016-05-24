using System;
using System.Data.Entity;
using System.Linq;

namespace Tavarta.Utility
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ToPagedQuery<T>(this IQueryable<T> query, int pageSize, int pageNumber)
        {
            if (pageSize < 0) throw new ArgumentOutOfRangeException(nameof(pageSize));
            var resultsToSkip = pageNumber * pageSize;
            return query.Skip(() => resultsToSkip).Take(() => pageSize);
        }
    }
}