using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IGenericRepository<T> where T : class  // É comum usarmos "T" para geérico
    {
        IQueryable<T> GetAll(); // Método que me devolve todas as entidades 
                                // Substitui o método IEnumerable - IRepository

        Task<T> GetByIdAsync(int id);  // Substitui o método GetProduct - IRepository


        Task CreateAsync (T entity); // Substitui o método AddProduct - IRepository


        Task UpdateAsync(T entity); // Substitui o método UpdateProduct - IRepository


        Task DeleteAsync (T entity); // Substitui o método RemoveProduct - IRepository


        Task<bool> ExistAsync (int id); // Substitui o método RemoveProduct - IRepository
    }
}
