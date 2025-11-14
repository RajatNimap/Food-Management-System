using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FOOD.MODEL.Model
{
    public class BaseEntityModel
    {
        
        public DateTime? CreatedDate { get; set; } 
        public DateTime? ModifiedDate { get; set; } 
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? ModifiedBy { get; set; }    

    }
}

