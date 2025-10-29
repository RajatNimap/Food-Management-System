using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FOOD.DATA.Entites
{
    public class BaseEntity
    {
       
       public int Id { get; set; }    
       public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
       public DateTime? ModifiedDate { get;set; } = DateTime.UtcNow; 
       public int? CreatedBy { get; set; }
       public int? ModifiedBy { get; set; }    

    }
}
