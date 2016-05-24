using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Tavarta.Common.Extentions
{
    public static class PagedResultExtentions
    {

        /// <summary>
        /// صفحه بندی کوئری
        /// </summary>
        /// <param name="query">کوئری مورد نظر شما</param>
        /// <param name="pageNum">شماره صفحه</param>
        /// <param name="pageSize">سایز صفحه</param>
        /// <param name="orderByProperty">ترتیب خواص</param>
        /// <param name="isAscendingOrder">اگر برابر با <c>true</c> باشد صعودی است</param>
        /// <param name="rowsCount">تعداد کل ردیف ها</param>
        /// <returns></returns>
        public static IQueryable<T> PagedResult<T, TResult>(this IQueryable<T> query, int pageNum, int pageSize,
                        Expression<Func<T, TResult>> orderByProperty, bool isAscendingOrder, out int rowsCount)
        {
            if (pageSize <= 0) pageSize = 20;

            //مجموع ردیف‌های به دست آمده
            rowsCount = query.Count();

            // اگر شماره صفحه کوچکتر از 0 بود صفحه اول نشان داده شود
            if (rowsCount <= pageSize || pageNum <= 0) pageNum = 1;

            // محاسبه ردیف هایی که نسبت به سایز صفحه باید از آنها گذشت
            int excludedRows = (pageNum - 1) * pageSize;

            query = isAscendingOrder ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);

            // ردشدن از ردیف‌های اضافی و  دریافت ردیف‌های مورد نظر برای صفحه مربوطه
            return query.Skip(() => excludedRows).Take(()=>pageSize);
        }

    }
}