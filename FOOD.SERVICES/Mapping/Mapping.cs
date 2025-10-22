using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;

namespace FOOD.SERVICES.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() { 
        
            CreateMap<User,UserModel>().ReverseMap();   
        }    

    }
}
