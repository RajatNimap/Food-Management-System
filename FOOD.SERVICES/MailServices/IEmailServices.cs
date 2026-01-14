using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.SERVICES.MailServices
{
    public interface IEmailServices
    {
        public Task SendEmailAsync(string toEmail, string subject, string body);  
    }
}
