using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Models;

namespace SuperShop.Helpers
{
    public interface IUserHelper
    {
        // ctrl. using Data.Entities
        Task<User> GetUserByEmailAsync(string email); //Verifica se o email e dá-me o User

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);  // Vídeo 16

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

    }
}
