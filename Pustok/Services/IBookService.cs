using Pustok.Models;
using PustokSliderCRUD.Models;

namespace Pustok.Services
{
    public interface IBookService
    {
        Task CreateAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
        
    }
}
