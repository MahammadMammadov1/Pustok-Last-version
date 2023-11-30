using Microsoft.AspNetCore.Mvc;
using Pustok.DAL;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _slider;

        public SliderController(AppDbContext appDb)
        {
            _slider = appDb;
        }
        public IActionResult Index()
        {
            List<Slider> sliderList = _slider.Sliders.ToList();
            return View(sliderList);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);
            string fileName = "";
            if (slider.FormFile != null)
            {
                fileName = slider.FormFile.FileName;
                if (slider.FormFile.ContentType != "image/jpeg" && slider.FormFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("FormFile", "you can only add png or jpeg file");
                }

                if (slider.FormFile.Length > 1048576)
                {
                    ModelState.AddModelError("FormFile", "file must be lower than 1 mb");
                }

                if (slider.FormFile.FileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }

                fileName = Guid.NewGuid().ToString() + fileName;

                string path = "C:\\Users\\Mehemmed\\Desktop\\CRUD-class-practice\\Pustok\\wwwroot\\uploads\\sliders\\" + fileName;
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    slider.FormFile.CopyTo(fileStream);
                }
                slider.ImageUrl = fileName;
            }
            else
            {
                ModelState.AddModelError("FormFile", "image is required");
            }




            _slider.Sliders.Add(slider);
            _slider.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var wanted = _slider.Sliders.FirstOrDefault(x => x.Id == id);

            return View(wanted);
        }
        [HttpPost]
        public IActionResult Update(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);
            var wanted = _slider.Sliders.FirstOrDefault(x => x.Id == slider.Id);

            if (wanted == null)
            {
                return NotFound();
            }
            string oldFilePath = "C:\\Users\\Mehemmed\\Desktop\\CRUD-class-practice\\Pustok\\wwwroot\\uploads\\sliders\\" + wanted.ImageUrl;

            if (slider.FormFile != null)
            {

                string newFileName = slider.FormFile.FileName;
                if (slider.FormFile.ContentType != "image/jpeg" && slider.FormFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("FormFile", "you can only add png or jpeg file");
                }

                if (slider.FormFile.Length > 1048576)
                {
                    ModelState.AddModelError("FormFile", "file must be lower than 1 mb");
                }

                if (slider.FormFile.FileName.Length > 64)
                {
                    newFileName = newFileName.Substring(newFileName.Length - 64, 64);
                }

                newFileName = Guid.NewGuid().ToString() + newFileName;

                string newFilePath = "C:\\Users\\Mehemmed\\Desktop\\CRUD-class-practice\\Pustok\\wwwroot\\uploads\\sliders\\" + newFileName;
                using (FileStream fileStream = new FileStream(newFilePath, FileMode.Create))
                {
                    slider.FormFile.CopyTo(fileStream);
                }

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                wanted.ImageUrl = newFileName;
            }

            wanted.Title = slider.Title;
            wanted.Description = slider.Description;
            wanted.RedirctText = slider.RedirctText;
            wanted.RedirctUrl = slider.RedirctUrl;

            _slider.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null) NotFound();
            var wanted = _slider.Sliders.FirstOrDefault(x => x.Id == id);
            if (wanted != null) NotFound();

            _slider.Sliders.Remove(wanted);
            _slider.SaveChanges();

            return View(wanted);
        }


    }
}
