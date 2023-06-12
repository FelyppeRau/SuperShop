using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;


namespace SuperShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper; // inserir o "_" depois de inserir o field

        public AccountController(IUserHelper userHelper) // ctrl. para inserir o field
        {
            _userHelper = userHelper;
        }


        // Aqui só aparece a View
        public IActionResult Login()  //"Botão Direito" -> AddView
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]  // Aqui é que de fato valida as informações
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl")) 
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First()); // Caso tente acessar outra view diferente do Login, sou direcionado para a view Login, mas após sou direcionado
                                                                                  // para a view que tentei acessar em primeiro lugar. Exemplo: ProductsController [Authorize]
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return View(model);
        }

        public async Task<IActionResult> Logout()  //"Botão Direito" -> AddView
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Register() //"Botão Direito" -> AddView
        {
            return View();
        }

        [HttpPost]  // Aqui é que de fato valida as informações
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.UserName);

                if(user == null)
                {
                    user = new User
                    {
                        FisrtName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.UserName,
                        UserName = model.UserName,

                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if(result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created");
                        return View(model);
                    }

                    var loginViewModel = new LoginViewModel
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        RememberMe = false,
                    };

                    var result2 = await _userHelper.LoginAsync(loginViewModel);

                    if(result2.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "The user couldn't be logged");
                    
                }

            }

            return View(model);

        }

    }



}
