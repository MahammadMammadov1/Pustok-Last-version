using Pustok.Models;
using PustokSliderCRUD.Models;

namespace Pustok.Repositories
{
    public interface IBookRepository
    {
        Task CreateAsync(Book book);
        Task CreateBookTagAsync(BookTag booktag);
        Task CreateBookImageAsync(BookImage bookimage);
        Task<Book> GetBookById(int id);
        void DeleteAsync(Book book);
        Task<List<Book>> GetAllBookAsync();
        Task<List<Tag>> GetAllTagAsync();
        Task<List<Genre>> GetAllGenreAsync();
        Task<List<BookTag>> GetAllBookTagAsync();
        Task<List<Author>> GetAllAuthorAsync();
        Task<List<BookImage>> GetAllBookImagesAsync();
        Task<int> SaveAsync();
    }
}
