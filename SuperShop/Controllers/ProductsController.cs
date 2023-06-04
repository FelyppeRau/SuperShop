using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {
        
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper; // inserir o "_" depois de inserir o field

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper) // ctrl. para inserir o field
        {
           
            _productRepository = productRepository;
            _userHelper = userHelper; // inserir o "_" depois de inserir o field
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_productRepository.GetAll().OrderBy(p => p.Name)); // ORDENAR!!!!!!!!!!!!!!!!!!!!!
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value); // como o parâmetro na linha 27 está definido como opcional (?) devemos por .Value
                                                                           // pois caso seja null ele saberá tratar e não arrebentará com a aplicação 
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
         
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if(model.ImageFile != null && model.ImageFile.Length > 0) // Verificar se tem imagem
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\products",
                        file);

                    using(var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                    
                    path = $"~/images/products/{file}";

                }

                var product = this.ToProduct(model, path);


                //TODO: Modificar para o User que estiver logado
                product.User = await _userHelper.GetUserByEmailAsync("felypperau@gmail.com");

                await _productRepository.CreateAsync(product);
                
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private Product ToProduct(ProductViewModel model, string path)
        {
            return new Product
            {
                Id = model.Id,
                ImageUrl = path,
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User,
            };
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            var model = this.ToProductViewModel(product);
            return View(model);
        }

        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                IsAvailable = product.IsAvailable,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Price  = product.Price,
                Stock = product.Stock,
                User = product.User,

            };
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {           
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;
                         
                    if(model.ImageFile != null && model.ImageFile.Length > 0) // Verificar se tem imagem
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\products",
                            file);

                        using(var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/products/{file}";
                    }

                    var product = this.ToProduct(model, path);

                    //TODO: Modificar para o User que estiver logado
                    product.User = await _userHelper.GetUserByEmailAsync("felypperau@gmail.com"); // Aqui devemos confirmar o User assim como no Post Create

                    await _productRepository.UpdateAsync(product);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            await _productRepository.DeleteAsync(product);            
            return RedirectToAction(nameof(Index));
        }

    }
}
