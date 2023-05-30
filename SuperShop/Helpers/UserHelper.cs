using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public class UserHelper : IUserHelper // ctrl. Implementar a Interface
    {
        private readonly UserManager<User> _userManager; // inserir o "_" depois de inserir o field

        public UserHelper(UserManager<User> userManager) // ctrl. para inserir o field
        {
            _userManager = userManager; // inserir o "_" depois de inserir o field
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)    
        {
            return await _userManager.CreateAsync(user, password); // *** CRIA O USER  ***
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email); // ***  EMAIL  ***
        }
    }
}
