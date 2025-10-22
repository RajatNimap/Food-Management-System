using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.MODEL.Model;

namespace FOOD.SERVICES.AuthenticationServices
{
    public interface IAuth
    {
        Task<string> IsAuthenticated(LoginModel login);   
    }
}
