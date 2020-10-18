using Stemma.Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stemma.Infrastructure.UtilityHelper
{
    public static class PagingExtention
    {
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query,
                                                int pageNo, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.PageNo = pageNo;
            result.PageSize = pageSize;
            result.TotalRecords = query.Count();

            var pageCount = (double)result.TotalRecords / pageSize;
            result.TotalPages = (int)Math.Ceiling(pageCount);

            var skip = (pageNo - 1) * pageSize;
            result.Records = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public static PagedResult<T> GetPaged<T>(this IList<T> query,
                                                 int pageNo, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.PageNo = pageNo;
            result.PageSize = pageSize;
            result.TotalRecords = query.Count();

            var pageCount = (double)result.TotalRecords / pageSize;
            result.TotalPages = (int)Math.Ceiling(pageCount);

            var skip = (pageNo - 1) * pageSize;
            result.Records = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        public async static Task<PagedResult<TSource>> GetPagedAsync<TSource>(this IQueryable<TSource> query, int pageNo, int pageSize) where TSource : class
        {
            var result = new PagedResult<TSource>();
            result.PageNo = pageNo;
            result.PageSize = pageSize;
            result.TotalRecords = query.Count();

            var pageCount = (double)result.TotalRecords / pageSize;
            result.TotalPages = (int)Math.Ceiling(pageCount);

            var skip = (pageNo - 1) * pageSize;
            result.Records = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }

        public async static Task<PagedResult<TSource>> GetPagedAsync<TSource>(this IList<TSource> query, int pageNO, int pageSize) where TSource : class
        {
            var result = new PagedResult<TSource>();
            result.PageNo = pageNO;
            result.PageSize = pageSize;
            result.TotalRecords = query.Count();

            var pageCount = (double)result.TotalRecords / pageSize;
            result.TotalPages = (int)Math.Ceiling(pageCount);

            var skip = (pageNO - 1) * pageSize;
            result.Records = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}
