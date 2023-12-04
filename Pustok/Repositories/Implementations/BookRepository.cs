using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using PustokSliderCRUD.Models;

namespace Pustok.Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _appDb;

        public BookRepository(AppDbContext appDb)
        {
            _appDb = appDb;
        }
        public async Task CreateAsync(Book book)
        {
            await _appDb.Books.AddAsync(book);
        }

        public async Task CreateBookImageAsync(BookImage bookimage)
        {
            await _appDb.BookImages.AddAsync(bookimage);
        }

        public async Task CreateBookTagAsync(BookTag booktag)
        {
            await _appDb.BookTags.AddAsync(booktag);
        }

        public void DeleteAsync(Book book)
        {
            _appDb.Books.Remove(book);
        }

        public async Task<List<Author>> GetAllAuthorAsync()
        {
            return await _appDb.Authors.ToListAsync();
        }

        public async Task<List<Book>> GetAllBookAsync()
        {
            return await _appDb.Books.ToListAsync();
        }

        public async Task<List<BookImage>> GetAllBookImagesAsync()
        {
            return await _appDb.BookImages.ToListAsync();
        }

        public async Task<List<BookTag>> GetAllBookTagAsync()
        {
            return await _appDb.BookTags.ToListAsync();
        }

        public async Task<List<Genre>> GetAllGenreAsync()
        {
            return await _appDb.Genres.ToListAsync();
        }

        public async Task<List<Tag>> GetAllTagAsync()
        {
            return await _appDb.Tags.ToListAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _appDb.Books.Include(x=>x.BookImages).Include(x => x.BookTags).Include(x => x.Author).Include(x => x.Genre).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int> SaveAsync()
        {
            return await _appDb.SaveChangesAsync();
        }
    }
}
