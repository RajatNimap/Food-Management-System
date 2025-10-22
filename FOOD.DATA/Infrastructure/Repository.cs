using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FOOD.DATA.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext dbcontext;
        private readonly DbSet<T> dbset;

        public Repository(DataContext _dbcontext)
        {
            dbcontext = _dbcontext;
            dbset = dbcontext.Set<T>();
        }   
        public async Task Add(T entity)
        {
            await dbset.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            dbset.Remove(entity);
            dbcontext.Entry(entity).State = EntityState.Deleted;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbset.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await dbset.FindAsync(id);   
        }

        public void Update(T entity)
        {
              dbset.Update(entity);
        }
    }
}
