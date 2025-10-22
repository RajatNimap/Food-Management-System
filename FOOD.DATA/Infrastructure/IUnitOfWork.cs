using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOOD.DATA.Repository.UserRepository;

namespace FOOD.DATA.Infrastructure
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public Task<int> Commit();
    }
}
