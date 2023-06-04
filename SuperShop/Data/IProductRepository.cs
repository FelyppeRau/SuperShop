using System.Linq;
using SuperShop.Data.Entities;


namespace SuperShop.Data
{
    public interface IProductRepository : IGenericRepository<Product> // (ctrl.) inserir using Data.Entities
    {
        public IQueryable GetAllWithUsers();
    }
}
