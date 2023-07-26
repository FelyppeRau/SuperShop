using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Data.Entities;


namespace SuperShop.Data
{
    public interface IProductRepository : IGenericRepository<Product> // (ctrl.) inserir using Data.Entities
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboProducts();
    }
}
