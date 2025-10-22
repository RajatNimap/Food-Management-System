using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.MODEL.Model;


namespace FOOD.SERVICES.UserServices
{
    public interface IUserServices
    {
        public Task<IEnumerable<User>> GetAllUser();
        public Task<User> GetSingleUser(int id);
        public Task AddUser(UserModel user);   
        public Task UpdateUser(int id,UserModel user);
        public Task DeleteUser(int id);

    }
}
