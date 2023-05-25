using System.Collections.Generic;
using System.Threading.Tasks;
using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    public interface IRepository
    {
        void AddProduct(Product product);

        Product GetProduct(int id);

        IEnumerable<Product> GetProducts();

        bool ProductExists(int id);

        void RmoveProduct(Product product);

        Task<bool> SaveAllAssync();

        void UpdateProduct(Product product);
    }
}