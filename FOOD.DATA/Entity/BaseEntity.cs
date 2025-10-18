﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.DATA.Entity
{
    public class BaseEntity
    {
       public int Id { get; set; }    
       public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
       public DateTime ModifiedDate { get;set; }
       public string CreatedBy { get; set; }
       public string ModifiedBy { get; set; }    

    }
}
