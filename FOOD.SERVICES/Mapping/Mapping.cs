using AutoMapper;
using FFOOD.MODEL.Model;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;
using FOOD.MODEL.ReportModel;
using FOOD.Utility;
using FOOD.Utility.Extension;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FOOD.SERVICES.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {



            CreateMap<User, UserModel>()
    .ForMember(dest => dest.CreatedDate,
        opt => opt.MapFrom(src => UtilityHelper.ToLocalDateandTime(src.CreatedDate)))
    .ForMember(dest => dest.ModifiedDate,
        opt => opt.MapFrom(src => UtilityHelper.ToLocalDateandTime(src.ModifiedDate)));

            // Model → Entity (REQUIRED for Add)
            CreateMap<UserModel, User>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            //  CreateMap<Orders,OrdersModel>().ReverseMap()
            CreateMap<Orders, OrdersModel>()
                  .ForMember(x => x.CreatedDate, y => y.MapFrom(src => UtilityHelper.ToLocalDateandTime(src.CreatedDate)))
                  .ForMember(z => z.ModifiedDate, opt => opt.MapFrom(src => UtilityHelper.ToLocalDateandTime(src.ModifiedDate)));

            CreateMap<OrdersModel, Orders>()
                 // you can control CreatedDate/ModifiedDate here as you wish:
                 .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                 .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            CreateMap<OrderItems, OrderItemsModel>()
               .ReverseMap();
            // CreateMap<Inventory, InventoryModel>().ReverseMap();
            CreateMap<Inventory, InventoryModel>()
                 .ForMember(x => x.CreatedDate, y => y.MapFrom(src => UtilityHelper.ToLocalDateandTime(src.CreatedDate)))
                 .ForMember(x => x.ModifiedDate, y => y.MapFrom(src => UtilityHelper.ToLocalDateandTime(src.ModifiedDate)));
            CreateMap<InventoryModel, Inventory>()
               // you can control CreatedDate/ModifiedDate here as you wish:
               .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
               .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

            CreateMap<Menu, MenuModel>()
                .ForMember(x => x.CreatedDate, y => y.MapFrom(src => UtilityHelper.ToLocalDateandTime(src.CreatedDate)))
                 .ForMember(x => x.ModifiedDate, y => y.MapFrom(src => UtilityHelper.ToLocalDateandTime(src.ModifiedDate)));
            CreateMap<MenuModel, Menu>()
    .ForMember(dest => dest.Id, opt => opt.Ignore());


            CreateMap<MenuModel, Menu>()
   // you can control CreatedDate/ModifiedDate here as you wish:
   .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
   .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());


            CreateMap<Recipe, RecipeModel>()
                .ForMember(x=>x.CreatedDate,y=>y.MapFrom(src=>UtilityHelper.ToLocalDateandTime(src.CreatedDate)));
            CreateMap<Inventory,InverntoryReportModel>().ReverseMap();

            CreateMap<RecipeModel, Recipe>()
   // you can control CreatedDate/ModifiedDate here as you wish:
   .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
        }


    }
}
