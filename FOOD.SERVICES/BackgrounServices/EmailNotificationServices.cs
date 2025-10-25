using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.SERVICES.MailServices;
using FOOD.SERVICES.Reports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FOOD.SERVICES.BackgrounServices
{
    public class LowStockEmailNotificationServices : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _recipientMail = "reetaroashan@gmail.com";
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(60);  

        public LowStockEmailNotificationServices(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) {

                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var lowStockReports = scope.ServiceProvider.GetRequiredService<ILowStockReports>();
                        var emailService = scope.ServiceProvider.GetRequiredService <IEmailServices>();
                        var data = await lowStockReports.GetLowStockReport();

                        if (data != null &&
                            ((data.CiticalItems > 0) || (data.lowStockSummaryModel?.WarningCount > 0)))
                        {
                            // Build email subject and body
                            var subject = " Low Stock Alert - Inventory Report";

                            var body = new StringBuilder();
                            body.AppendLine("<h2>Low Stock Alert</h2>");
                            body.AppendLine($"<p>Report Generated At: {data.GeneratedAt}</p>");
                            body.AppendLine("<h3>Summary:</h3>");
                            body.AppendLine($"<ul>");
                            body.AppendLine($"<li><b>Total Items Checked:</b> {data.lowStockSummaryModel.TotalItmes}</li>");
                            body.AppendLine($"<li><b>Critical Items:</b> {data.lowStockSummaryModel.CriticalCount}</li>");
                            body.AppendLine($"<li><b>Warning Items:</b> {data.lowStockSummaryModel.WarningCount}</li>");
                            body.AppendLine($"</ul>");

                            body.AppendLine("<h3>Low Stock Items:</h3>");
                            body.AppendLine("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;'>");
                            body.AppendLine("<tr><th>Item Name</th><th>Available</th><th>Reorder Level</th><th>Status</th></tr>");

                            foreach (var item in data.LowStockItems)
                            {
                                body.AppendLine($"<tr>");
                                body.AppendLine($"<td>{item.ItemName}</td>");
                                body.AppendLine($"<td>{item.QuantityAvailable} {item.unit}</td>");
                                body.AppendLine($"<td>{item.ReorderLevel}</td>");
                                body.AppendLine($"<td>{item.Status}</td>");
                                body.AppendLine($"</tr>");
                            }

                            body.AppendLine("</table>");
                            body.AppendLine("<br><p>Please take action to restock these items.</p>");
                            body.AppendLine("<p>Thank you,<br><b>Inventory Management System</b></p>");

                            // Send the email
                            await emailService.SendEmailAsync(
                                _recipientMail,
                                subject,
                                body.ToString()
                            );
                        }


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending low stock notification: {ex.Message}");

                }
                await Task.Delay(_checkInterval, stoppingToken);    
            }
        }
    }
}
