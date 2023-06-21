using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ctrl. para adicionar o using no "User"
            // Caso não utilizemos outra propriedade (ex.: FirstName) não precisamos criar a classe e aqui utilizamos IdentityUser (classe do ASPNET.CORE), ao invés de User
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true; // Email único
                cfg.Password.RequireDigit = false; // Desativado as premissas da password.  QUANDO ESTIVER ONLINE DEVEMOS ALTERAR POR CAUSA DA SEGURANÇA
                cfg.Password.RequiredUniqueChars = 0; // Aqui colocamos a quantidade de caracteres especiais
                cfg.Password.RequireUppercase = false; // Não obriga ter uma letra maiúscula.
                cfg.Password.RequireLowercase = false; // Não obriga ter uma letra minúscula
                cfg.Password.RequireNonAlphanumeric = false; // Não obriga ter caracteres alfa-numéricos
                cfg.Password.RequiredLength = 6; // Determina o tamanho da password                
            })
                .AddEntityFrameworkStores<DataContext>();
            
            
            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<SeedDb>();

            services.AddScoped<IUserHelper, UserHelper>();

            //services.AddScoped<IImageHelper, ImageHelper>(); //RETIRADO APÓS O BLOB AZURE

            services.AddScoped<IBlobHelper, BlobHelper>();

            services.AddScoped<IConverterHelper, ConverterHelper>();

            services.AddScoped<IProductRepository, ProductRepository>(); // ** Aqui podemos fazer testes.. / Modificar o "Repository"

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/NotAuthorized";   //Aparece a View quw criamos. É o contrário do ReturnUrl
                options.AccessDeniedPath = "/Account/NotAuthorized";  //Aparece a View quw criamos. É o contrário do ReturnUrl
            });

            services.AddControllersWithViews(); 



            // Transient cria o objecto e depois deita fora... Neste caso aqui só volta a criá-lo quando eu corro a aplicação

            //services.AddSingleton<> // Singlenton cria o objecto e mantém ele na aplicação

            //services.AddScoped // Scoped cria o objecto e já está instanciado. Quando volto a criá-lo novamente, deita fora o que lá está e sobrepõe

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //DEVE ESTAR EM ORDEM

            app.UseStatusCodePagesWithReExecute("/error/{0}"); // Devemos fazer a IAction no primeiro controlador que arranca a aplicação (IActionResult Error404())

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Servirá para o Login. DEVE estar ANTES do Authorization()

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
