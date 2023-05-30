using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;


namespace SuperShop.Helpers
{
    public interface IUserHelper
    {
        // ctrl. using Data.Entities
        Task<User> GetUserByEmailAsync(string email); //Verifica se o email e dá-me o User

        Task<IdentityResult> AddUserAsync(User user, string password);

    }
}
