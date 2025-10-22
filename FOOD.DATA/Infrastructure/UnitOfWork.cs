using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Repository.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace FOOD.DATA.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
       //public IUserRepository UserRepository { get; }
        private DataContext dbcontext; 
        private IUserRepository userRepository;
       public UnitOfWork(DataContext _dbcontext)
       {
           // UserRepository = userepo;
            dbcontext=_dbcontext;
       }
                
        public IUserRepository UserRepository { 

            get{ 
                
                return userRepository = userRepository ?? new UserRepository(dbcontext);
            } 
        }    

        public async Task<int> Commit()
        {
            return await dbcontext.SaveChangesAsync();
        }
    }
}
