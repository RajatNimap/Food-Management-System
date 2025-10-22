using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.Model;
using Microsoft.EntityFrameworkCore;

namespace FOOD.SERVICES.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrdersModel>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetAll();
                return _mapper.Map<IEnumerable<OrdersModel>>(orders);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving all orders", ex);
            }
        }

        public async Task<OrdersModel> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetById(id);
                if (order == null)
                    throw new KeyNotFoundException($"Order with ID {id} not found");

                return _mapper.Map<OrdersModel>(order);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while retrieving order with ID {id}", ex);
            }
        }

        public async Task<bool> CreateOrderAsync(OrdersModel orderModel)
        {
            try
            {
                orderModel.CreatedDate = DateTime.UtcNow;
                orderModel.CreatedBy = "System";

                var orderEntity = _mapper.Map<Orders>(orderModel);
                await _unitOfWork.OrderRepository.Add(orderEntity);

                var rowsAffected = await _unitOfWork.Commit();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while creating order", ex);
            }
        }

        public async Task<bool> UpdateOrderAsync(int id, OrdersModel orderModel)
        {
            try
            {
                var existingOrder = await _unitOfWork.OrderRepository.GetById(id);
                if (existingOrder == null)
                    throw new KeyNotFoundException($"Order with ID {id} not found");

                _mapper.Map(orderModel, existingOrder);
                existingOrder.ModifiedDate = DateTime.UtcNow;
                existingOrder.ModifiedBy = "System";

                var rowsAffected = await _unitOfWork.Commit();
                return rowsAffected > 0;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while updating order with ID {id}", ex);
            }
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetById(id);
                if (order == null)
                    throw new KeyNotFoundException($"Order with ID {id} not found");

                _unitOfWork.OrderRepository.Delete(order);
                var rowsAffected = await _unitOfWork.Commit();
                return rowsAffected > 0;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while deleting order with ID {id}", ex);
            }
        }

        public Task<OrderCreatedResult> PlacingOrder(Orders order)
        {
            try
            {
                using (var transaction = awai  _unitOfWork.BeginTransactionAsync())
                {

                    decimal totalAmt = 0;
                    var shortageItems = new List<ShortageItem>();

                    foreach (var orderItem in order.OrderItems)
                    {

                        var menu = await dbcontext.menus.Include(x => x.Recipes).ThenInclude(y => y.Inventory).FirstOrDefaultAsync(x => x.Id == orderItem.MenuId);
                        if (menu == null)
                        {
                            throw new ArgumentException($"Menu with ID {orderItem.MenuId} not found");
                        }
                        foreach (var item in menu.Recipes)
                        {
                            //  var inventory = await dbcontext.inventories.FirstOrDefaultAsync(x => x.Id == item.ItemId);
                            var inventory = item.Inventory;
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
                        if (shortageItems.Any())
                        {
                            await transaction.RollbackAsync();
                            return new OrderCreatedResult
                            {
                                IsSuccess = false,
                                Message = "Insuffcient inventory for some item",
                                ShortageItems = shortageItems
                            };

                        }
                        foreach (var recipe in menu.Recipes)
                        {

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
            catch (Exception ex)
            {

                throw new Exception("Error occurred while creating order", ex);

            }


        }
    }
}