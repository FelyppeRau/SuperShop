using System;
using System.Linq;
using System.Threading.Tasks;
using SuperShop.Data.Entities;


namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private Random _ramdom;

        public SeedDb(DataContext context)
        {
            _context = context;
            _ramdom = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); // Verifica se a BD está criada. Se não estiver, CRIA. Caso já esteja criada, segue...

            if(!_context.Products.Any())
            {
                AddProduct("IPhone X");
                AddProduct("Magic Mouse");
                AddProduct("IWatch Series 4");
                AddProduct("IPad Mini");

                await _context.SaveChangesAsync();  // AQUI É QUE GRAVA NA BD
            }
        }

        private void AddProduct(string name)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _ramdom.Next(1000),
                IsAvailable = true,
                Stock = _ramdom.Next(100),
            });

        }
    }
}
