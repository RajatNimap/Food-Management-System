using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FFOOD.MODEL.Model;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;
using FOOD.MODEL.ReportModel;

namespace FOOD.SERVICES.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {

            CreateMap<User, UserModel>().ReverseMap();

            CreateMap<Orders,OrdersModel>().ReverseMap();
            CreateMap<OrderItems, OrderItemsModel>()
               .ReverseMap();
            CreateMap<Inventory, InventoryModel>().ReverseMap();
            CreateMap<Menu, MenuModel>().ReverseMap();
            CreateMap<Recipe, RecipeModel>().ReverseMap();
            CreateMap<Inventory,InverntoryReportModel>().ReverseMap(); 
        }

    }
}
