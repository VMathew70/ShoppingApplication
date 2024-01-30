using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.ProdDAL
{
    public class ProdDBContext : DbContext
    {
        public ProdDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }

        public DbSet<OrderDetail> OrderDetail { get; set; }


    }
}
