using Microsoft.EntityFrameworkCore;
using FOOD.DATA.Entites;    

namespace FOOD.DATA
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> users { get; set; }
    }
}
