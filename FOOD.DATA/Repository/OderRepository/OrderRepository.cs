using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace FOOD.DATA.Repository.OderRepository
{
    public class OrderRepository : Repository<Orders>, IOrderRepository
    {
        private readonly DataContext dbcontext;
        public OrderRepository(DataContext dbcontext): base(dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<OrderCreatedResult> CreateOrders(Orders order)
        {
            try
            {
                using (var transaction = await dbcontext.Database.BeginTransactionAsync())
                {

                    decimal totalAmt = 0;
                    var shortageItems = new List<ShortageItem>();

                    foreach (var orderItem in order.OrderItems)
                    {

                        var menu = await dbcontext.menus.Include(x=>x.Recipes).ThenInclude(y=>y.Inventory).FirstOrDefaultAsync(x => x.Id == orderItem.MenuId);
                        if (menu == null)
                        {
                            throw new ArgumentException($"Menu with ID {orderItem.MenuId} not found");
                        }
                        foreach (var item in menu.Recipes)
                        {
                          //  var inventory = await dbcontext.inventories.FirstOrDefaultAsync(x => x.Id == item.ItemId);
                             var inventory =item.Inventory;    
                            if (inventory == null)
                            {
                                throw new ArgumentException($"Inventory item with ID {item.ItemId} not found");

                            }
                            decimal requiredquantity = item.QuantityRequired * orderItem.QuantityOrdered;
                            if (inventory.QuantityAvailable < requiredquantity)
                            {
                                
                                shortageItems.Add(new ShortageItem
                                {
                                    ItemId = inventory.Id,
                                    ItemName = inventory.ItemName,
                                    RequiredQuantity = requiredquantity,
                                    AvailableQuantity = inventory.QuantityAvailable,
                                    ShortageQuantity = requiredquantity - inventory.QuantityAvailable,
                                    MenuId = menu.Id,
                                    MenuName = menu.MenuName
                                });
                            }

                        }
                        if (shortageItems.Any()) { 
                            await transaction.RollbackAsync();
                            return new OrderCreatedResult
                            {
                                IsSuccess = false,
                                Message = "Insuffcient inventory for some item",
                                ShortageItems = shortageItems   
                            };
                        
                        }
                        foreach (var recipe in menu.Recipes) { 

                            var inventory = recipe.Inventory;
                            decimal requiredquantity = recipe.QuantityRequired * orderItem.QuantityOrdered;
                            inventory.QuantityAvailable -= requiredquantity;
                            dbcontext.inventories.Update(inventory);

                        }
                        await dbcontext.orderItems.AddAsync(orderItem); 
                        totalAmt += (orderItem.UnitPrice * orderItem.QuantityOrdered);
                    }
                    order.TotalAmount = totalAmt;
                    await dbcontext.orders.AddAsync(order);
                    await dbcontext.SaveChangesAsync(); 
                    await transaction.CommitAsync();

                    return new OrderCreatedResult
                    {
                        IsSuccess = true,
                        Message = "Order created successfully",
                        OrderId = order.Id,
                        order = order,
                        ShortageItems = new List<ShortageItem>()
                    };
                }

            }
            catch (Exception ex) {

                throw new Exception("Error occurred while creating order", ex);

            }


        }
    }
}
