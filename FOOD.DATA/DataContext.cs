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
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<Orders> orders { get; set; }   
        public DbSet<OrderItems> orderItems { get; set; }   
        public DbSet<Recipe> recipes { get; set; }
        public DbSet<Menu> menus { get; set; }
    }
}
