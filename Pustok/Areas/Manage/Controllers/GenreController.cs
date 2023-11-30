using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;


namespace PustokSliderCRUD.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class GenreController : Controller
    {
        private readonly AppDbContext _appDb;

        public GenreController(AppDbContext context)
        {
            _appDb = context;
        }
        public IActionResult Index()
        {
            var genres = _appDb.Genres.ToList();
            return View(genres);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Genre genre)
        {
            if (!ModelState.IsValid) return View(genre);
            if (_appDb.Genres.Any(x => x.Name.ToLower().Trim() == genre.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name", "Genre alredy exist!!!");
                return View(genre);
            }
            _appDb.Genres.Add(genre);
            _appDb.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Genre wanted = _appDb.Genres.FirstOrDefault(x => x.Id == id);
            if (wanted == null) return NotFound();
            return View(wanted);
        }
        [HttpPost]
        public IActionResult Update(Genre genre)
        {
            Genre existGenre = _appDb.Genres.FirstOrDefault(x => x.Id == genre.Id);
            if (existGenre == null) return NotFound();
            if (!ModelState.IsValid) return View(existGenre);
            if (_appDb.Genres.Any(x => x.Id != genre.Id && x.Name.ToLower().Trim() == genre.Name.ToLower().Trim()))
            {
                ModelState.AddModelError("Name", "Genre alredy exist!!!");
                return View(genre);
            }
            existGenre.Name = genre.Name;
            _appDb.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Genre genre = _appDb.Genres.FirstOrDefault(g => g.Id == id);

            if (genre == null) return NotFound();
            _appDb.Genres.Remove(genre);
            _appDb.SaveChanges();

            return Ok();
        }
    }
}