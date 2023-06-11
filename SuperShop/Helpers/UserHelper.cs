using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public class UserHelper : IUserHelper // ctrl. Implementar a Interface
    {
        private readonly UserManager<User> _userManager; // inserir o "_" depois de inserir o field
        private readonly SignInManager<User> _signInManager;

        public UserHelper(UserManager<User> userManager, SignInManager<User> signInManager) // ctrl. para inserir o field
        {
            _userManager = userManager; // inserir o "_" depois de inserir o field
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)    
        {
            return await _userManager.CreateAsync(user, password); // *** CRIA O USER  ***
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email); // ***  EMAIL  ***
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
