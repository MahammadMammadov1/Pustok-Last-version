using Microsoft.AspNetCore.Mvc;
using Pustok.DAL;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers
{
    
        [Area("Manage")]
        public class AuthorController : Controller
        {
            private readonly AppDbContext _appDb;

            public AuthorController(AppDbContext context)
            {
                _appDb = context;
            }
            public IActionResult Index()
            {
                var author = _appDb.Authors.ToList();
                return View(author);
            }
            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
            public IActionResult Create(Author author)
            {
                if (!ModelState.IsValid) return View(author);
                
                _appDb.Authors.Add(author);
                _appDb.SaveChanges();
                return RedirectToAction("Index");
            }
            public IActionResult Update(int id)
            {
                var wanted = _appDb.Authors.FirstOrDefault(x => x.Id == id);
                if (wanted == null) return NotFound();
                return View(wanted);
            }
            [HttpPost]
            public IActionResult Update(Author author)
            {
                Author existAuthor = _appDb.Authors.FirstOrDefault(x => x.Id == author.Id);
                if (existAuthor == null) return NotFound();
                if (!ModelState.IsValid) return View(existAuthor);
               
                existAuthor.Name = author.Name;
                _appDb.SaveChanges();
                return RedirectToAction("Index");
            }
            public IActionResult Delete(int id)
            {
                var wanted = _appDb.Authors.FirstOrDefault(x => x.Id == id);
                if (wanted == null) return NotFound();
                return View(wanted);
            }
            [HttpPost]
            public IActionResult Delete(Author author)
            {
                var wanted = _appDb.Authors.FirstOrDefault(x => x.Id == author.Id);
                if (wanted == null) return NotFound();
                _appDb.Authors.Remove(wanted);
                _appDb.SaveChanges();
                return RedirectToAction("Index");
            }
        }
}
