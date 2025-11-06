using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;

namespace FOOD.SERVICES.AuthenticationServices
{
    public interface IJwtService
    {
         string JwtAccessToken(User user);  
         string JwtRefreshToken(); 
    }
}
