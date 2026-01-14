using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;

namespace FOOD.DATA.Repository.RefreshTokenRepository
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>    
    {
        Task<RefreshToken?> IsValidToken(string refreshToken);   
    }
}
