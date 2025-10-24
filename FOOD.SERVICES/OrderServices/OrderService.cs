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

        public async Task<bool> UpdateOrder(OrdersModel orderModel)
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
        public async Task<OrderCreatedResult> PlacingOrder(OrdersModel order)
        {
            try
            {
                    bool IsMoveFuther = true;
                    decimal totalAmt = 0;
                    var shortageItems = new List<ShortageItem>();

                    foreach (var orderItem in order.OrderItems)
                    {

                         var menu = await _unitOfWork.OrderRepository.GetMenuWithRecipesAsync(orderItem.MenuId);
                         if (menu == null)
                         {
                            throw new ArgumentException($"Menu with ID {orderItem.MenuId} not found");
                         }
                        foreach (var item in menu.Recipes)
                        {
                            var inventory = item.Inventory;
                            if (inventory == null)
                            {
                                throw new ArgumentException($"Inventory item with ID {item.ItemId} not found");

                            }
                                 decimal requiredquantity = item.QuantityRequired * orderItem.QuantityOrdered;
                                 if (inventory.QuantityAvailable < requiredquantity)
                                 {
                                    IsMoveFuther = false;

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
                            if (IsMoveFuther)
                            {
                                inventory.QuantityAvailable -= requiredquantity;
                                _unitOfWork.InventoryRepository.Update(inventory);
                            }
                        

                        }

                        if (IsMoveFuther)
                                totalAmt += (orderItem.UnitPrice * orderItem.QuantityOrdered);
                    }

                if (shortageItems.Any())
                {
                    return new OrderCreatedResult
                    {
                        IsSuccess = false,
                        Message = "Insuffcient inventory for some item",
                        ShortageItems = _mapper.Map<List<ShortageItem>>(shortageItems)
                    };

                }
                else
                {
                    order.TotalAmount = totalAmt;
                    var MappedOrder = _mapper.Map<Orders>(order);
                    await _unitOfWork.OrderRepository.Add(MappedOrder);
                    await _unitOfWork.Commit();

                    return new OrderCreatedResult
                    {
                        IsSuccess = true,
                        Message = "Order created successfully",
                        OrderId = MappedOrder.Id,
                        order = order,
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