using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FOOD.MODEL.HelperModel;
using Microsoft.AspNetCore.Http;

namespace FOOD.SERVICES.HttpContext
{
    public class UserContext:IUserContext
    {
        private readonly IHttpContextAccessor _contexAccess;

        public UserContext(IHttpContextAccessor contexAccess)
        {
            _contexAccess = contexAccess;
        }

        public int? UserId { get { 
            
            var userId = _contexAccess.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(userId, out var id) ? id : (int?)null;  
            }
        }

        public string UserName { 
            get
            {
                return _contexAccess.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            }
        } 
        public bool IsAuthenticated
        {
            get
            {
                return _contexAccess.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
            }
        }

     
        public int GetCurrentUserId()
        {
            if(!IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            return UserId ?? throw new InvalidOperationException("User ID is not available.");  
        }
    }
}
