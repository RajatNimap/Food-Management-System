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
        
       [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public DateTime ModifiedDate { get;set; } 
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }    

    }
}

