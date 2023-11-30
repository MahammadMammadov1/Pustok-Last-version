using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using PustokSliderCRUD.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
        private readonly AppDbContext _appDb;

        public BookController(AppDbContext appDb)
        {
            _appDb = appDb;
        }
        public IActionResult Index()
        {

            var book = _appDb.Books.ToList();
            return View(book);
        }
        public IActionResult Create()
        {
            ViewBag.Authors = _appDb.Authors.ToList();
            ViewBag.Genres = _appDb.Genres.ToList();
            ViewBag.Tags = _appDb.Tags.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {
            ViewBag.Authors = _appDb.Authors.ToList();
            ViewBag.Genres = _appDb.Genres.ToList();
            ViewBag.Tags = _appDb.Tags.ToList();

            if (!ModelState.IsValid) return View(book);
            if (!_appDb.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author not found!!!");
                return View();
            }
            if (!_appDb.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found!!!");
                return View();
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
                ModelState.AddModelError("TagId", "Tag not found!");
                return View(book);
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
                        _appDb.BookTags.Add(bookTag);
                    }
                }
            }

            if (book.BookPoster != null)
            {
                string fileName = book.BookPoster.FileName;
                if (book.BookPoster.ContentType != "image/jpeg" && book.BookPoster.ContentType != "image/png")
                {
                    ModelState.AddModelError("BookPoster", "you can only add png or jpeg file");
                }

                if (book.BookPoster.Length > 1048576)
                {
                    ModelState.AddModelError("BookPoster", "file must be lower than 1 mb");
                }

                if (book.BookPoster.FileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }

                fileName = Guid.NewGuid().ToString() + fileName;

                string path = "C:\\Users\\II Novbe\\Desktop\\Pustok-File-\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
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

                _appDb.BookImages.Add(bookImage);

            }
            else
            {
                ModelState.AddModelError("BookPoster", "image is required");
            }



            if (book.BookHower != null)
            {
                string fileName = book.BookHower.FileName;
                if (book.BookHower.ContentType != "image/jpeg" && book.BookHower.ContentType != "image/png")
                {
                    ModelState.AddModelError("BookHower", "you can only add png or jpeg file");
                }

                if (book.BookHower.Length > 1048576)
                {
                    ModelState.AddModelError("BookHower", "file must be lower than 1 mb");
                }

                if (book.BookHower.FileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }

                fileName = Guid.NewGuid().ToString() + fileName;

                string path = "C:\\Users\\II Novbe\\Desktop\\Pustok-File-\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
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
                _appDb.BookImages.Add(bookImage);
            }
            else
            {
                ModelState.AddModelError("BookHower", "image is required");
            }


            if (book.ImageFiles !=null)
            {
                foreach (var img in book.ImageFiles)
                {
                    string fileName = img.FileName;
                    if (img.ContentType != "image/jpeg" && img.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ImageFiles", "you can only add png or jpeg file");
                    }

                    if (img.Length > 1048576)
                    {
                        ModelState.AddModelError("ImageFiles", "file must be lower than 1 mb");
                    }

                    if (img.FileName.Length > 64)
                    {
                        fileName = fileName.Substring(fileName.Length - 64, 64);
                    }

                    fileName = Guid.NewGuid().ToString() + fileName;

                    string path = "C:\\Users\\II Novbe\\Desktop\\Pustok-File-\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
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
                    _appDb.BookImages.Add(bookImage);
                }
            }


            _appDb.Books.Add(book);
            _appDb.SaveChanges();
            return RedirectToAction("Index");
        }



        
    

        public IActionResult Update(int id)
        {
            ViewBag.Authors = _appDb.Authors.ToList();
            ViewBag.Genres = _appDb.Genres.ToList();
            ViewBag.Tags = _appDb.Tags.ToList();

            if (!ModelState.IsValid) return View();
            var existBook = _appDb.Books.Include(x => x.BookTags).Include(x => x.BookImages).FirstOrDefault(x => x.Id == id);
            return View(existBook);
        }

        [HttpPost]
        public IActionResult Update(Book book)
        {
            ViewBag.Authors = _appDb.Authors.ToList();
            ViewBag.Genres = _appDb.Genres.ToList();
            ViewBag.Tags = _appDb.Tags.ToList();


            var existBook = _appDb.Books.Include(x=> x.BookTags).Include(x=>x.BookImages).FirstOrDefault(b => b.Id == book.Id);
            if (existBook == null) return NotFound();
            if (!ModelState.IsValid) return View(book);
            if (!_appDb.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author not found!!!");
                return View();
            }
            if (!_appDb.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found!!!");
                return View();
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
                    ModelState.AddModelError("BookPoster", "you can only add png or jpeg file");
                }

                if (book.BookPoster.Length > 1048576)
                {
                    ModelState.AddModelError("BookPoster", "file must be lower than 1 mb");
                }

                if (book.BookPoster.FileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }

                fileName = Guid.NewGuid().ToString() + fileName;

                string path = "C:\\Users\\II Novbe\\Desktop\\Pustok-File-\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
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
                ModelState.AddModelError("BookPoster", "image is required");
            }



            if (book.BookHower != null)
            {
                string fileName = book.BookHower.FileName;
                if (book.BookHower.ContentType != "image/jpeg" && book.BookHower.ContentType != "image/png")
                {
                    ModelState.AddModelError("BookHower", "you can only add png or jpeg file");
                }

                if (book.BookHower.Length > 1048576)
                {
                    ModelState.AddModelError("BookHower", "file must be lower than 1 mb");
                }

                if (book.BookHower.FileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }

                fileName = Guid.NewGuid().ToString() + fileName;

                string path = "C:\\Users\\II Novbe\\Desktop\\Pustok-File-\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
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
                ModelState.AddModelError("BookHower", "image is required");
            }

            
            if (book.ImageFiles != null)
            {
                if (book.BookImages != null )
                {
                    existBook.BookImages.RemoveAll(bi => !book.BookImageIds.Contains(bi.Id) && bi.isPoster == null);
                }
                foreach (var img in book.ImageFiles)
                {
                    string fileName = img.FileName;
                    if (img.ContentType != "image/jpeg" && img.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ImageFiles", "you can only add png or jpeg file");
                    }

                    if (img.Length > 1048576)
                    {
                        ModelState.AddModelError("ImageFiles", "file must be lower than 1 mb");
                    }

                    if (img.FileName.Length > 64)
                    {
                        fileName = fileName.Substring(fileName.Length - 64, 64);
                    }

                    fileName = Guid.NewGuid().ToString() + fileName;

                    string path = "C:\\Users\\II Novbe\\Desktop\\Pustok-File-\\Pustok\\wwwroot\\uploads\\books\\" + fileName;
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
            _appDb.SaveChanges();
            return RedirectToAction("Index");
        }
       
        [HttpGet]
        public IActionResult Delete(Book book)
        {
            var wanted = _appDb.Books.FirstOrDefault(x => x.Id == book.Id);

            if (wanted == null) return NotFound();

            _appDb.Books.Remove(wanted);
            _appDb.SaveChanges();

            return Ok();
        }
    }
}
