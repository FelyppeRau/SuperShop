using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Helpers; 

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;        
        //private readonly UserManager<User> _userManager; // inserir o "_" depois de inserir o field
        private readonly IUserHelper _userHelper; // inserir o "_" depois de inserir o field
        private Random _ramdom;

        // public SeedDb(DataContext context, UserManager<User> userManager) // Caso não queiramos o mostrar o User: FirstName e LastName (Propriedade criada além do default - Class User)  basta não colocar a Entity/Classe "User"
                                                                             // ctrl. para inserir o field
        public SeedDb(DataContext context, IUserHelper userHelper) // ctrl. para inserir o field
        {
            _context = context;
            _userHelper = userHelper; // inserir o "_" depois de inserir o field
            //_userManager = userManager; // inserir o "_" depois de inserir o field
            _ramdom = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); // Verifica se a BD está criada. Se não estiver, CRIA. Caso já esteja criada, segue...

            //***CRIA A BD PRIMEIRO DEPOIS OS USERS***

            // var user = await _userManager.FindByEmailAsync("felypperau@gmail.com"); // Verifica se o User existe. Se for null, cria o User.
            var user = await _userHelper.GetUserByEmailAsync("felypperau@gmail.com"); // Verifica se o User existe. Se for null, cria o User.

            if (user == null)
            {
                user = new User
                {
                    FisrtName = "Jorge",
                    LastName = "Pinto",
                    Email = "felypperau@gmail.com",
                    UserName = "felypperau@gmail.com",
                    PhoneNumber = "234567890",
                };

                // Caso não tenha o User informado, CRIAMOS  USER!!
                // var result = await _userManager.CreateAsync(user, "123456"); // A password sempre está a parte do objecto para ser encriptada. Nunca no User
                var result = await _userHelper.AddUserAsync(user, "123456"); // A password sempre está a parte do objecto para ser encriptada. Nunca no User

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder"); // Caso não crie o User saberemos onde o programa estourou. Me dá uma exeção
                }
            }

            if (!_context.Products.Any()) 
            {
                AddProduct("IPhone X", user); // Após criarmos a Classe User, inserimos o parâmetro - Video 10 27:00
                AddProduct("Magic Mouse", user);
                AddProduct("IWatch Series 4", user);
                AddProduct("IPad Mini", user);

                await _context.SaveChangesAsync();  // AQUI É QUE GRAVA NA BD
            }
        }

        private void AddProduct(string name, User user) // Após criarmos a Classe User, inserimos o parâmetro - Video 10 27:30
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _ramdom.Next(1000), // preço aleatorio 
                IsAvailable = true,
                Stock = _ramdom.Next(100), // quantidade aleatoria
                User = user, // Após criarmos a Classe User, inserimos o parâmetro - Video 10 27:30

            });

        }
    }
}
