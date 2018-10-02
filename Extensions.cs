using System;
using System.Linq;

namespace ExtensionMethod
{
    using DrankReus_api.Models;
    public static class Extensions
    {
        public static Page<T> GetPage<T>(this Microsoft.EntityFrameworkCore.DbSet<T> list, int page_index,
            int page_size, Func<T, object> order_by_selector) where T : class
        {
            var res = list.OrderBy(order_by_selector)
                .Skip(page_index * page_size)
                .Take(page_size)
                .ToArray();
            if (res == null || res.Length == 0)
            {
                return null;
            }

            var total_items = list.Count();
            var total_pages = total_items / page_size;
            if (total_items < total_pages) 
                total_pages = 1;
            
            return new Page<T>(){Index = page_index, Items = res, TotalPages = total_pages};
        }
        public static Page<T> GetPage<T>(this IQueryable<T> query, int page_index,
            int page_size, Func<T, object> order_by_selector) where T : class 
        {
            var res = query.AsEnumerable().OrderBy(order_by_selector)
                .Skip(page_index * page_size)
                .Take(page_size)
                .ToArray();
            if (res == null || res.Length == 0)
            {
                return null;
            }

            var total_items = query.Count();
            var total_pages = total_items / page_size;
            if (total_items < total_pages) 
                total_pages = 1;
            
            return new Page<T>(){Index = page_index, Items = res, TotalPages = total_pages};
        }
    }
}