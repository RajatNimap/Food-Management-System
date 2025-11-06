using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FOOD.MODEL.Pagination;
using Microsoft.EntityFrameworkCore;

namespace FOOD.SERVICES.Pagination
{
   public static class QueryableExtensions
    {
            public static  async Task<PaginationModel<T>> PagedResult<T,TKey>(
                
                this IQueryable<T> queryable,   
                int pageNumber,
                int pageSize,
                Expression<Func<T, TKey>> orderByExpression
             )
            {
                    var TotalRecords =await queryable.CountAsync();

                    var items = await queryable
                         .OrderBy(orderByExpression)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                    
                return new PaginationModel<T>
                {
                    Items = items,
                    PageNumer = pageNumber,
                    PageSize = pageSize,
                    TotalRecord = TotalRecords
                };
        }
    }
}
