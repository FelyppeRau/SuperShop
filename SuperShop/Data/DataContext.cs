using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    public class DataContext : IdentityDbContext<User> // Instalar o package Identity.EntityFrameworkCore
    {

        public DbSet<Product> Products { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

    }
}
