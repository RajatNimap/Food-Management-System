using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;

namespace FOOD.DATA.Repository.UserRepository
{
    public interface IUserRepository:IRepository<User>
    {
        //public Task<User?> VerifyUser(LoginModel login);
        public Task<User> verifyMail(string email);

    }
}