using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.Model;

namespace FOOD.DATA.Repository.OderRepository
{
    public interface IOrderRepository : IRepository<Orders>
    {
        public Task<OrderCreatedResult> CreateOrders(Orders model);
    }
}
