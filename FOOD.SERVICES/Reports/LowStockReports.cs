using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FOOD.DATA.Infrastructure;
using FOOD.MODEL.ReportModel;

namespace FOOD.SERVICES.Reports
{

    public class LowStockReports : ILowStockReports
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LowStockReports(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

     

        public async Task<LowStockReportModel> GetLowStockReport()
        {
            try
            {
                var lowStockItems = await _unitOfWork.InventoryRepository.GetLowStockItems();
                return new LowStockReportModel
                {
                    GeneratedAt = DateTime.UtcNow,
                    TotalLowStockItems = lowStockItems.Count(),
                    CiticalItems = lowStockItems.Count(i => i.QuantityAvailable <= 0),
                    LowStockItems = _mapper.Map<ICollection<InverntoryReportModel>>(lowStockItems),
                    lowStockSummaryModel = new LowStockSummaryModel
                    {
                        TotalItmes = lowStockItems.Count(),
                        CriticalCount = lowStockItems.Count(i => i.QuantityAvailable <= 0),
                        WarningCount = lowStockItems.Count(i => i.QuantityAvailable > 0 && i.QuantityAvailable <= i.ReorderLevel)
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while generating low stock report", ex);
            }


        }
    }
}
