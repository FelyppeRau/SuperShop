using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperShop.Data.Entities;


namespace SuperShop.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        // Construtordo repositório
        public Repository(DataContext context) // (ctrl.) para creiar o field / Altera para "_context"
        {
            _context = context;
        }

        // (ctrl.) para buscar o using Entities / using Collections.Generic 
        public IEnumerable<Product> GetProducts() // Aqui buscamos TODOS os productos

        {
            return _context.Products.OrderBy(p => p.Name);
        }

        public Product GetProduct(int id) // Aqui buscamos UM os producto
        {
            return _context.Products.Find(id);
        }

        public void AddProduct(Product product) // Adicionar um producto (em memória)
        {
            _context.Products.Add(product);
        }

        public void UpdateProduct(Product product) // UpDate do producto (em memória)
        {
            _context.Products.Update(product);
        }



        public void RmoveProduct(Product product) // Remove o producto (em memória)
        {
            _context.Products.Remove(product);
        }

        public async Task<bool> SaveAllAssync() // Aqui que GRAVA NA BD.
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool ProductExists(int id) // Verifica se o producto existe
        {
            return _context.Products.Any(p => p.Id == id);
        }
    }
}
