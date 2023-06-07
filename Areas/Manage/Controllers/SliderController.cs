using Microsoft.AspNetCore.Mvc;
using ProniaProject.Areas.Manage.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Helpers;
using ProniaProject.Models;

namespace ProniaProject.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SliderController : Controller
    {
        private readonly ProniaContext _context;
        public IWebHostEnvironment _env { get; }

        public SliderController(ProniaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1)
        {
            var query = _context.Sliders.OrderBy(x => x.Order).AsQueryable();

            var data = PaginatedList<Slider>.Create(query, page, 2);

            if (data.TotalPage < page)
            {
                string url = Url.Action("index", "slider", new { page = data.TotalPage });
                return Redirect(url);
            }

            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.NextOrder = _context.Sliders.Any() ? _context.Sliders.Max(x => x.Order) + 1 : 1;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            ViewBag.NextOrder = slider.Order;
            if (!ModelState.IsValid) return View();

            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required");
                return View();
            }

            foreach (var item in _context.Sliders.Where(x => x.Order >= slider.Order))
                item.Order++;

            slider.ImageName = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            slider.BgImageName = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.BgImageFile);

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null) return View("Error");

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            Slider existSlider = _context.Sliders.Find(slider.Id);

            if (existSlider == null) return View("Error");

            existSlider.Order = slider.Order;
            existSlider.Title = slider.Title;
            existSlider.Offer = slider.Offer;
            existSlider.BtnUrl = slider.BtnUrl;
            existSlider.BtnText = slider.BtnText;
            existSlider.Desc = slider.Desc;

            string oldFileName = null;
            if (slider.ImageFile != null)
            {
                oldFileName = existSlider.ImageName;
                existSlider.ImageName = FileManager.Save(_env.WebRootPath, "uploads/sliders", slider.ImageFile);
            }

            _context.SaveChanges();

            if (oldFileName != null)
                FileManager.Delete(_env.WebRootPath, "uploads/sliders", oldFileName);

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null) return StatusCode(404);
            FileManager.Delete(_env.WebRootPath, "uploads/sliders", slider.ImageName);
            FileManager.Delete(_env.WebRootPath, "uploads/sliders", slider.BgImageName);
            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
