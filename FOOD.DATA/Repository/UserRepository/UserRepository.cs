using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Entites;
using FOOD.DATA.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FOOD.DATA.Repository.UserRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DataContext dbcontext;
        public UserRepository(DataContext dbcontext): base(dbcontext)
        {
           this.dbcontext = dbcontext;   
        }

        public async Task<User?> verifyMail(string email)
        {
            return await dbcontext.users.FirstOrDefaultAsync(x => x.Email == email);
        }



        //public async Task<User?> VerifyUser(LoginModel login)
        //{
        //    var user = await dbcontext.users.FirstOrDefaultAsync(x => x.Email == login.Email);
        //    if (user == null) {

        //         return null;      

        //    }
        //    var verifyCredential = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
        //    return verifyCredential ? user : null;

        //}

    }
}
