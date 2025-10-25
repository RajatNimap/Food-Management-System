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
            try
            {
                using (var excelPackage = new ExcelPackage())
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Daily Report");
                    var data = await _orderReport.DailyReport(date);

                    worksheet.Cells[2, 2].Value = "Report Date:";
                    worksheet.Cells[3, 2].Value = "Total Orders:";
                    worksheet.Cells[4, 2].Value = "Total Items:";
                    worksheet.Cells[5, 2].Value = "Total Revenue:";

                    worksheet.Cells[2, 3].Value = data.ReportDate.ToString("dd-MM-yyyy");
                    worksheet.Cells[3, 3].Value = data.TotalOrders;
                    worksheet.Cells[4, 3].Value = data.TotalItemsSold;
                    worksheet.Cells[5, 3].Value = data.TotalRevenue;
                    worksheet.Cells[5, 3].Style.Numberformat.Format = "₹#,##0.00";

                    using (var range = worksheet.Cells[2, 2, 5, 3])
                    {
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }

                    worksheet.Cells[8, 2].Value = "Order ID";
                    worksheet.Cells[8, 3].Value = "Customer";
                    worksheet.Cells[8, 4].Value = "Menu Item";
                    worksheet.Cells[8, 5].Value = "Quantity";
                    worksheet.Cells[8, 6].Value = "Unit Price";
                    worksheet.Cells[8, 7].Value = "Item Total";
                    worksheet.Cells[8, 8].Value = "Order Total";

                    using (var header = worksheet.Cells[8, 2, 8, 8])
                    {
                        header.Style.Font.Bold = true;
                        header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        header.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    int startRow = 9;

                    foreach (var order in data.Orders)
                    {
                        int itemCount = order.OrderItems.Count;
                        int endRow = startRow + itemCount - 1;

                        if (itemCount > 1)
                        {
                            worksheet.Cells[startRow, 2, endRow, 2].Merge = true; 
                            worksheet.Cells[startRow, 3, endRow, 3].Merge = true; 
                            worksheet.Cells[startRow, 8, endRow, 8].Merge = true; 
                        }

                        worksheet.Cells[startRow, 2].Value = order.OrderId;
                        worksheet.Cells[startRow, 3].Value = order.CustomerName;
                        worksheet.Cells[startRow, 8].Value = order.TotalAmount;
                        worksheet.Cells[startRow, 8].Style.Numberformat.Format = "₹#,##0.00";

                        int currentRow = startRow;
                        foreach (var item in order.OrderItems)
                        {
                            worksheet.Cells[currentRow, 4].Value = item.MenuName;
                            worksheet.Cells[currentRow, 5].Value = item.Quantity;
                            worksheet.Cells[currentRow, 6].Value = item.unitprice;
                            worksheet.Cells[currentRow, 6].Style.Numberformat.Format = "₹#,##0.00";
                            worksheet.Cells[currentRow, 7].Value = item.Quantity * item.unitprice;
                            worksheet.Cells[currentRow, 7].Style.Numberformat.Format = "₹#,##0.00";
                            currentRow++;

                        }

                        startRow = endRow + 1;
                    }

                    using (var range = worksheet.Cells[8, 2, startRow - 1, 8])
                    {
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }

                    worksheet.Cells.AutoFitColumns();

                    return excelPackage.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while exporting daily report to Excel", ex);
            }
        }

    }
}