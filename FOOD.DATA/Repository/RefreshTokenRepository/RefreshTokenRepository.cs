using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FOOD.DATA.Repository.RefreshTokenRepository
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly DataContext _dbcontext; 
        public RefreshTokenRepository(DataContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext; 
        }

        public async Task<RefreshToken?> IsValidToken(string refreshToken)
        {
            return  await _dbcontext.refreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken && x.ExpiresDate > DateTime.UtcNow && x.IsRevoked == false);
        }
      
    }
}
