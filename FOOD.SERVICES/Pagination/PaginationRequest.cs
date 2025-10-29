using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.SERVICES.Pagination
{
    public class PaginationRequest
    {
        private int pagenumber = 1;
        private int pagesize = 10;  
        public int PageNumber
        {
            get{ pagenumber=(pagenumber < 1) ? 1 :pagenumber; }
            set { pagenumber = (value < 1) ? 1 : value; }
        }   
    }
}
