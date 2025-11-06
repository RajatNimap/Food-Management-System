using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.MODEL.Pagination
{
    public class PaginationModel<T>
    {
        public IReadOnlyList<T> Items { get; set; }
        public int TotalRecord { get; set; } 
        public int PageNumer { get; set; }
        public int PageSize { get; set; }
        public  int TotalPages => (int)Math.Ceiling((double)TotalRecord / PageSize);    
    }
}
