using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.MODEL.HelperModel
{
    public interface IUserContext
    {
        int? UserId { get; }
        string UserName { get; }
        bool IsAuthenticated { get; }
        int GetCurrentUserId(); 
    }
}
