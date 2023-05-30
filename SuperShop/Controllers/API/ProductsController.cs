using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;

namespace SuperShop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository; // inserir o "_" depois de inserir o field
         
        public ProductsController(IProductRepository productRepository) // ctrl. para inserir o field
        {
            _productRepository = productRepository; // inserir o "_" depois de inserir o field
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_productRepository.GetAll()); // Busca todos os productos dentro do repositório e o (OK) embrulha tudo em Json
        }
    }
}
