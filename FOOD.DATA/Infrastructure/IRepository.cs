using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOOD.DATA.Infrastructure
{
    public interface IRepository<T> where T : class 
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);    
        Task Update(T entity);  
        Task Add(T entity); 
        Task Delete(T entity);

    }
}
