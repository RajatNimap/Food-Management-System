using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.ReportModel;

namespace FOOD.SERVICES.Reports
{
    public class DailyOrderReport : IDailyOrderReport
    {
        private readonly IUnitOfWork _unitOfWork;
        public DailyOrderReport(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    
        public async Task<DailyReportModel> DailyReport(DateOnly reportDate)
        {
            try
            {
                var OrdersPlaced = await _unitOfWork.OrderRepository.GetMenuWithRecipesDateTime(reportDate);

                DailyReportModel report = new DailyReportModel
                {
                    ReportDate = reportDate,
                    TotalOrders = OrdersPlaced.Count(),
                    TotalItemsSold = OrdersPlaced.Sum(o => o.OrderItems.Sum(oi => oi.QuantityOrdered)),
                    TotalRevenue = OrdersPlaced.Sum(o => o.TotalAmount),
                    Orders = OrdersPlaced.Select(o => new ReportOrderItemModel
                    {
                        OrderId = o.Id,
                        CustomerName = o.CustomerName,
                        TotalAmount = o.TotalAmount,
                        OrderItems = o.OrderItems.Select(oi => new ReportSummaryModel
                        {
                            MenuName = oi.Menu.MenuName,
                            Quantity = oi.QuantityOrdered,
                            unitprice = oi.UnitPrice,
                        }).ToList()
                    }).ToList()
                };

                return report;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while generating daily report", ex);
            }



        }
    }
}

