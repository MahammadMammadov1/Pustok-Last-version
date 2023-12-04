using Pustok.DAL;
using Pustok.Exceptions;
using Pustok.Models;
using Pustok.Repositories;
using PustokSliderCRUD.Models;

namespace Pustok.Services.Implementations
{
    public class BookService : IBookService
    {
       
        private readonly AppDbContext _appDb;
        private readonly IBookRepository _bookRepository;

        public BookService(AppDbContext appDb,IBookRepository bookRepository)
        {
            
            _appDb = appDb;
            _bookRepository = bookRepository;
        }
        public async Task CreateAsync(Book book)
        {
            
            if (!_appDb.Authors.Any(x => x.Id == book.AuthorId))
            {
                throw new TotalBookExceptions("AuthorId", "Author not found!!!");
            }
            if (!_appDb.Genres.Any(x => x.Id == book.GenreId))
            {
                throw new TotalBookExceptions("GenreId", "Genre not found!!!");
                
            }
            var check = false;
            if (book.TagIds != null)
            {
                foreach (var tagId in book.TagIds)
                {
                    if (!_appDb.Tags.Any(x => x.Id == tagId))
                        check = true;
                }
            }
            if (check)
            {
                throw new TotalBookExceptions("TagId", "Tag not found!");
                
            }
            else
            {
                if (book.TagIds != null)
                {
                    foreach (var tagId in book.TagIds)
                    {
                        BookTag bookTag = new BookTag
                        {
                            Book = book,
                            TagId = tagId
                        };
                        await _bookRepository.CreateBookTagAsync(bookTag);
                    }
                }
            }

            if (book.BookPoster != null)
            {
                string fileName = book.BookPoster.FileName;
                if (book.BookPoster.ContentType != "image/jpeg" && book.BookPoster.ContentType != "image/png")
                {
                    throw new TotalBookExceptions("BookPoster", "you can only add png or jpeg file");
                }

                if (book.BookPoster.Length > 1048576)
                {
                    throw new TotalBookExceptions("BookPoster", "file must be lower than 1 mb");
                }

                if (book.BookPoster.FileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }

                fileName = Guid.NewGuid().ToString() + fileName;

                string path = "C:\\Users\\Mehemmed\\Desktop\\Pustok-Last-version\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    book.BookPoster.CopyTo(fileStream);
                }

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    ImageUrl = fileName,
                    isPoster = true,
                };

                await _bookRepository.CreateBookImageAsync(bookImage);

            }
            else
            {
                throw new TotalBookExceptions("BookPoster", "image is required");
            }



            if (book.BookHower != null)
            {
                string fileName = book.BookHower.FileName;
                if (book.BookHower.ContentType != "image/jpeg" && book.BookHower.ContentType != "image/png")
                {
                    throw new TotalBookExceptions("BookHower", "you can only add png or jpeg file");
                }

                if (book.BookHower.Length > 1048576)
                {
                    throw new TotalBookExceptions("BookHower", "file must be lower than 1 mb");
                }

                if (book.BookHower.FileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }

                fileName = Guid.NewGuid().ToString() + fileName;

                string path = "C:\\Users\\Mehemmed\\Desktop\\Pustok-Last-version\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    book.BookHower.CopyTo(fileStream);
                }

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    ImageUrl = fileName,
                    isPoster = false,
                };
                await _bookRepository.CreateBookImageAsync(bookImage);
            }
            else
            {
                throw new TotalBookExceptions("BookHower", "image is required");
            }


            if (book.ImageFiles != null)
            {
                foreach (var img in book.ImageFiles)
                {
                    string fileName = img.FileName;
                    if (img.ContentType != "image/jpeg" && img.ContentType != "image/png")
                    {
                        throw new TotalBookExceptions("ImageFiles", "you can only add png or jpeg file");
                    }

                    if (img.Length > 1048576)
                    {
                        throw new TotalBookExceptions("ImageFiles", "file must be lower than 1 mb");
                    }

                    if (img.FileName.Length > 64)
                    {
                        fileName = fileName.Substring(fileName.Length - 64, 64);
                    }

                    fileName = Guid.NewGuid().ToString() + fileName;

                    string path = "C:\\Users\\Mehemmed\\Desktop\\Pustok-Last-version\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        img.CopyTo(fileStream);
                    }

                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        ImageUrl = fileName,
                        isPoster = null,
                    };
                     await _bookRepository.CreateBookImageAsync(bookImage);
                }
            }


            await _bookRepository.CreateAsync(book);
            await _bookRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id == null) throw new NullReferenceException();

            Book wantedBook = await _bookRepository.GetBookById(id);

            if (wantedBook == null) throw new NullReferenceException();

           

            _bookRepository.DeleteAsync(wantedBook);
            await _bookRepository.SaveAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            var existBook = await _bookRepository.GetBookById(book.Id);
            if (existBook == null) throw new NullReferenceException();
            if (!_appDb.Authors.Any(x => x.Id == book.AuthorId))
            {
                throw new TotalBookExceptions("AuthorId", "Author not found!!!");
                
            }
            if (!_appDb.Genres.Any(x => x.Id == book.GenreId))
            {
                throw new TotalBookExceptions("GenreId", "Genre not found!!!");
                
            }


            if (book.TagIds != null)
            {
                existBook.BookTags.RemoveAll(bt => book.TagIds == null || !book.TagIds.Contains(bt.TagId));
                foreach (var tagId in book.TagIds.Where(tagId => !existBook.BookTags.Any(bt => bt.TagId == tagId)))
                {
                    BookTag bookTag = new BookTag
                    {
                        TagId = tagId
                    };
                    existBook.BookTags.Add(bookTag);
                }
            }



            if (book.BookPoster != null)
            {
                string fileName = book.BookPoster.FileName;
                if (book.BookPoster.ContentType != "image/jpeg" && book.BookPoster.ContentType != "image/png")
                {
                    throw new TotalBookExceptions("BookPoster", "you can only add png or jpeg file");
                }

                if (book.BookPoster.Length > 1048576)
                {
                    throw new TotalBookExceptions("BookPoster", "file must be lower than 1 mb");
                }

                if (book.BookPoster.FileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }

                fileName = Guid.NewGuid().ToString() + fileName;

                string path = "C:\\Users\\Mehemmed\\Desktop\\Pustok-Last-version\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    book.BookPoster.CopyTo(fileStream);
                }

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    ImageUrl = fileName,
                    isPoster = true,
                };

                existBook.BookImages.Add(bookImage);

            }
            else
            {
                throw new TotalBookExceptions("BookPoster", "image is required");
            }



            if (book.BookHower != null)
            {
                string fileName = book.BookHower.FileName;
                if (book.BookHower.ContentType != "image/jpeg" && book.BookHower.ContentType != "image/png")
                {
                    throw new TotalBookExceptions("BookHower", "you can only add png or jpeg file");
                }

                if (book.BookHower.Length > 1048576)
                {
                    throw new TotalBookExceptions("BookHower", "file must be lower than 1 mb");
                }

                if (book.BookHower.FileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }

                fileName = Guid.NewGuid().ToString() + fileName;

                string path = "C:\\Users\\Mehemmed\\Desktop\\Pustok-Last-version\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    book.BookHower.CopyTo(fileStream);
                }

                BookImage bookImage = new BookImage
                {
                    Book = book,
                    ImageUrl = fileName,
                    isPoster = false,
                };
                existBook.BookImages.Add(bookImage);
            }
            else
            {
                throw new TotalBookExceptions("BookHower", "image is required");
            }


            if (book.ImageFiles != null)
            {
                if (book.BookImages != null)
                {
                    existBook.BookImages.RemoveAll(bi => !book.BookImageIds.Contains(bi.Id) && bi.isPoster == null);
                }
                foreach (var img in book.ImageFiles)
                {
                    string fileName = img.FileName;
                    if (img.ContentType != "image/jpeg" && img.ContentType != "image/png")
                    {
                        throw new TotalBookExceptions("ImageFiles", "you can only add png or jpeg file");
                    }

                    if (img.Length > 1048576)
                    {
                        throw new TotalBookExceptions("ImageFiles", "file must be lower than 1 mb");
                    }

                    if (img.FileName.Length > 64)
                    {
                        fileName = fileName.Substring(fileName.Length - 64, 64);
                    }

                    fileName = Guid.NewGuid().ToString() + fileName;

                    string path = "C:\\Users\\Mehemmed\\Desktop\\Pustok-Last-version\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        img.CopyTo(fileStream);
                    }

                    BookImage bookImage = new BookImage
                    {
                        Book = book,
                        ImageUrl = fileName,
                        isPoster = null,
                    };
                    existBook.BookImages.Add(bookImage);
                }
            }


            existBook.Name = book.Name;
            existBook.Description = book.Description;
            existBook.CostPrice = book.CostPrice;
            existBook.DiscountedPrice = book.DiscountedPrice;
            existBook.Code = book.Code;
            existBook.SalePrice = book.SalePrice;
            existBook.Tax = book.Tax;
            existBook.IsAvailable = book.IsAvailable;
            existBook.AuthorId = book.AuthorId;
            existBook.GenreId = book.GenreId;
        }
    }
}
