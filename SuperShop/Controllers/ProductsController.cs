using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;


namespace SuperShop.Controllers
{
    /*[Authorize] */// SIGNIFICA SE SÓ PODERÃO ENTRAR NOS PRODUTOS OS QUE FIZEREM LOGIN VÁLIDO
    public class ProductsController : Controller
    {
        // inserir o "_" depois de inserir o field
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;
        //private readonly IImageHelper _imageHelper; // RETIRADO APÓS O BLOB AZURE
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper) // ctrl. para inserir o field
        {
            // inserir o "_" depois de inserir o field
            _productRepository = productRepository;
            _userHelper = userHelper; 
            //_imageHelper = imageHelper; // RETIRADO APÓS O BLOB AZURE
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
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
                //return NotFound(); //Alterado qndo criamos o NotFoundViewResult - Video 21
                return new NotFoundViewResult("ProductNotFound"); // Aqui passamos as Views que desejamos
            }

            var product = await _productRepository.GetByIdAsync(id.Value); // como o parâmetro na linha 27 está definido como opcional (?) devemos por .Value
                                                                           // pois caso seja null ele saberá tratar e não arrebentará com a aplicação 
            if (product == null)
            {
                //return NotFound(); //Alterado qndo criamos o NotFoundViewResult - Video 21
                return new NotFoundViewResult("ProductNotFound"); // Aqui passamos as Views que desejamos
            }

            return View(product);
        }

        // GET: Products/Create
        //[Authorize(Roles = "Admin")]
        [Authorize]
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
                //var path = string.Empty; // RETIRADO APÓS O BLOB AZURE

                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0) // Verificar se tem imagem
                {

                    //path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");  // RETIRADO APÓS O BLOB AZURE
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");

                }

                var product = _converterHelper.ToProduct(model, imageId, true); // True porque é novo


                //TODOoooo: Modificar para o User que estiver logado
                //product.User = await _userHelper.GetUserByEmailAsync("felypperau@gmail.com");

                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                await _productRepository.CreateAsync(product);
                
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                //return NotFound(); //Alterado qndo criamos o NotFoundViewResult - Video 21
                return new NotFoundViewResult("ProductNotFound"); // Aqui passamos as Views que desejamos
            }

            var product = await _productRepository.GetByIdAsync(id.Value);

            if (product == null)
            {
                //return NotFound(); //Alterado qndo criamos o NotFoundViewResult - Video 21
                return new NotFoundViewResult("ProductNotFound"); // Aqui passamos as Views que desejamos
            }

           
            var model = _converterHelper.ToProductViewModel(product);
            return View(model);
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
                    //var path = model.ImageUrl;
                    Guid imageId = model.ImageId;
                         
                    if(model.ImageFile != null && model.ImageFile.Length > 0) // Verificar se tem imagem
                    {

                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                    }

                    var product = _converterHelper.ToProduct(model, imageId, false); // False porque não é novo.


                    //TODOooo: Modificar para o User que estiver logado
                    //product.User = await _userHelper.GetUserByEmailAsync("felypperau@gmail.com"); // Aqui devemos confirmar o User assim como no Post Create

                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    await _productRepository.UpdateAsync(product);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistAsync(model.Id))
                    {
                        //return NotFound(); //Alterado qndo criamos o NotFoundViewResult - Video 21
                        return new NotFoundViewResult("ProductNotFound"); // Aqui passamos as Views que desejamos
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                //return NotFound(); //Alterado qndo criamos o NotFoundViewResult - Video 21
                return new NotFoundViewResult("ProductNotFound"); // Aqui passamos as Views que desejamos
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                //return NotFound(); //Alterado qndo criamos o NotFoundViewResult - Video 21
                return new NotFoundViewResult("ProductNotFound"); // Aqui passamos as Views que desejamos
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            try
            {
                //throw new Exception("Exceção de Teste");
                await _productRepository.DeleteAsync(product);
                return RedirectToAction(nameof(Index));
            }    
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{product.Name} provavelmente está a ser usado!!!";
                    ViewBag.ErrorMessage = $"{product.Name} não pode ser apagado visto haverem encomendas que o usam. </br></br>" +
                        $"Experimente primeiro apagar todas as encomendas que o estão a usar," +
                        $"e torne novamente a apagá-lo";
                }
                return View("Error");
            }
            
            
        }

        public IActionResult ProductNotFound()
        {
            return View();
        }

    }
}
