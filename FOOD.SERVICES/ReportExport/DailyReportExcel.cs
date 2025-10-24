using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FOOD.SERVICES.Reports;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using FOOD.MODEL.ReportModel;

namespace FOOD.SERVICES.ReportExport
{
    public class DailyReportExcel : IDailyReportExcel
    {
        private readonly IDailyOrderReport _orderReport;
        public DailyReportExcel(IDailyOrderReport orderReport)
        {
            _orderReport = orderReport;
        }

        public async Task<byte[]> DailyExport(DateOnly date)
        {

            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Daily Report");
                var data = await _orderReport.DailyReport(date);

                // Add summary information
                worksheet.Cells[2, 2].Value = "Report Date:";
                worksheet.Cells[3, 2].Value = "Total Orders:";
                worksheet.Cells[4, 2].Value = "Total Items:";
                worksheet.Cells[5, 2].Value = "Total Revenue:";

                worksheet.Cells[2, 3].Value = data.ReportDate.ToString("dd-MM-yyyy");
                worksheet.Cells[3, 3].Value = data.TotalOrders;
                worksheet.Cells[4, 3].Value = data.TotalItemsSold;
                worksheet.Cells[5, 3].Value = data.TotalRevenue;
                worksheet.Cells[5, 3].Style.Numberformat.Format = "$#,##0.00";

                using (var summaryRange = worksheet.Cells[2, 2, 5, 3])
                {
                    summaryRange.Style.Font.Bold = true;
                    summaryRange.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                }

                CreateOrderTable(worksheet, data.Orders, 8);

                // Auto-fit columns for better appearance
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return excelPackage.GetAsByteArray();
            }
        }

        private void CreateOrderTable(ExcelWorksheet worksheet, ICollection<ReportOrderItemModel> orders, int startRow)
        {
            // Define headers
            string[] headers = { "Order ID", "Customer", "Menu Item", "Quantity", "Unit Price", "Item Total", "Order Total" };

            int currentRow = startRow;

            foreach (var order in orders)
            {
                int orderStartRow = currentRow;

                // Create header row for each order group
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[currentRow, 2 + i].Value = headers[i];
                }

                // Style header row
                using (var headerRange = worksheet.Cells[currentRow, 2, currentRow, 2 + headers.Length - 1])
                {
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                currentRow++;

                // Add order items
                int itemStartRow = currentRow;
                foreach (var item in order.OrderItems)
                {
                    worksheet.Cells[currentRow, 2].Value = order.OrderId;           // Order ID
                    worksheet.Cells[currentRow, 3].Value = order.CustomerName;      // Customer
                    worksheet.Cells[currentRow, 4].Value = item.MenuName;           // Menu Item
                    worksheet.Cells[currentRow, 5].Value = item.Quantity;           // Quantity
                    worksheet.Cells[currentRow, 6].Value = item.unitprice;          // Unit Price
                    worksheet.Cells[currentRow, 6].Style.Numberformat.Format = "$#,##0.00";
                    worksheet.Cells[currentRow, 7].Value = item.Quantity * item.unitprice; // Item Total
                    worksheet.Cells[currentRow, 7].Style.Numberformat.Format = "$#,##0.00";

                    currentRow++;
                }

                int itemEndRow = currentRow - 1;

                // Merge Order ID and Customer cells vertically
                if (order.OrderItems.Count > 1)
                {
                    worksheet.Cells[itemStartRow, 2, itemEndRow, 2].Merge = true; // Order ID
                    worksheet.Cells[itemStartRow, 3, itemEndRow, 3].Merge = true; // Customer
                    worksheet.Cells[itemStartRow, 8, itemEndRow, 8].Merge = true; // Order Total

                    // Center the merged cells vertically and horizontally
                    worksheet.Cells[itemStartRow, 2, itemEndRow, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[itemStartRow, 3, itemEndRow, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[itemStartRow, 8, itemEndRow, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                // Add Order Total to the merged cell
                worksheet.Cells[itemStartRow, 8].Value = order.TotalAmount;
                worksheet.Cells[itemStartRow, 8].Style.Numberformat.Format = "$#,##0.00";
                worksheet.Cells[itemStartRow, 8].Style.Font.Bold = true;

                // Add borders to the entire order group
                using (var orderRange = worksheet.Cells[orderStartRow, 2, itemEndRow, 8])
                {
                    orderRange.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    orderRange.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    orderRange.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    orderRange.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                }

                // Add internal borders for the data rows
                using (var dataRange = worksheet.Cells[itemStartRow, 2, itemEndRow, 8])
                {
                    dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                // Add separator between orders
                currentRow += 2;
            }
        }
    }
}