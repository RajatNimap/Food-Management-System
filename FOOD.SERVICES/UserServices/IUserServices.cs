using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;
using FOOD.MODEL.Pagination;


namespace FOOD.SERVICES.UserServices
{
    public interface IUserServices
    {
        public Task<IEnumerable<User>> GetAllUser();
        public Task<UserModel> GetSingleUser(int id);
        public Task<bool> AddUser(UserModel user);   
        public Task<bool> UpdateUser(int id,UserModel user);
        public Task<bool> DeleteUser(int id);
        public Task<PaginationModel<UserModel>> GetAllUserPagination(int pageNumber,int pageSize);
    }
}
