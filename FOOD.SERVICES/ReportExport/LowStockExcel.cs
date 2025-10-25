using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.SERVICES.Reports;
using OfficeOpenXml;

namespace FOOD.SERVICES.ReportExport
{
    public class LowStockExcel : ILowStockExcel
    {
        public readonly ILowStockReports _lowStockReports;
        public LowStockExcel(ILowStockReports lowStockReports)
        {
            _lowStockReports = lowStockReports;
        }   
        public async Task<byte[]> LowStockExport()
        {
            try
            {

                using (var pakage = new ExcelPackage())
                {
                    var ws = pakage.Workbook.Worksheets.Add("Low Stock Report");
                    var data = await _lowStockReports.GetLowStockReport();

                    ws.Cells[2, 2].Value = "Generated Date";
                    ws.Cells[3, 2].Value = "Total Low StockItem";
                    ws.Cells[4, 2].Value = "Critical Count";
                    ws.Cells[5, 2].Value = "Warning Count";

                    ws.Cells[2, 3].Value = data.GeneratedAt.ToString("dd-MM-yyyy");
                    ws.Cells[3, 3].Value = data.TotalLowStockItems;
                    ws.Cells[4, 3].Value = data.CiticalItems;
                    ws.Cells[5, 3].Value = data.lowStockSummaryModel.WarningCount;

                    ws.Cells[8, 2].Value = "Id";
                    ws.Cells[8, 3].Value = "ItemName";
                    ws.Cells[8, 4].Value = "Unit";
                    ws.Cells[8, 5].Value = "Quantity Available";
                    ws.Cells[8, 6].Value = "Recorded Level";
                    ws.Cells[8, 7].Value = "Stock Deficit";
                    ws.Cells[8, 8].Value = "Status";

                    var startRow = 9;
                    foreach(var item in data.LowStockItems)
                    {
                        ws.Cells[startRow, 2].Value = item.Id;
                        ws.Cells[startRow, 3].Value = item.ItemName;
                        ws.Cells[startRow, 4].Value = item.unit;
                        ws.Cells[startRow, 5].Value = item.QuantityAvailable;
                        ws.Cells[startRow, 6].Value = item.ReorderLevel;
                        ws.Cells[startRow, 7].Value = item.StockDeficit;
                        ws.Cells[startRow, 8].Value = item.Status;
                        startRow++;
                    }

                    ws.Cells.AutoFitColumns();
                    return pakage.GetAsByteArray();
                }


            }
            catch(Exception ex)
            {
                throw new Exception("Error occurred while exporting Low Stock Report to Excel", ex);

            }
        }
    }
}
