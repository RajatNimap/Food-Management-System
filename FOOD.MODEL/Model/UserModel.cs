using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FOOD.MODEL.Model
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserModel:BaseEntityModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }
        [JsonIgnore]
        public int IsActive { get; set; } = 1;
    }
}
