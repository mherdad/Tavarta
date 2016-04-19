using System.Data.Entity;
using Tavarta.Utility;

namespace Tavarta.DataLayer
{
    public static class HintExtension
    {
        public static DbSet<T> WithHint<T>(this DbSet<T> set, string hint) where T : class
        {
            HintInterceptor.HintValue = hint;
            return set;
        }
        public static IDbSet<T> WithHint<T>(this IDbSet<T> set, string hint) where T : class
        {
            HintInterceptor.HintValue = hint;
            return set;
        }
    }
}