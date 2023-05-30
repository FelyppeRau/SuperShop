using Microsoft.AspNetCore.Identity;

namespace SuperShop.Data.Entities
{
    public class User : IdentityUser
    {
        public string FisrtName { get; set; }

        public string LastName { get; set; }

    }
}
