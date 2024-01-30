using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.ProdDAL
{
    public class OrderDBContext : DbContext
    {
        public OrderDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<OrderHeader> Orders { get; set; }
    }
}
