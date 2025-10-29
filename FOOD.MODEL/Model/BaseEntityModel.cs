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
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? ModifiedBy { get; set; }    

    }
}

