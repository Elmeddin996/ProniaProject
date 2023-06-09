using Microsoft.AspNetCore.Mvc;
using ProniaProject.Areas.Manage.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Helpers;
using ProniaProject.Models;

namespace ProniaProject.Areas.Manage.Controllers
{

    [Area("manage")]
    public class BannerController : Controller
    {
        private readonly ProniaContext _context;
        private readonly IWebHostEnvironment _env;

        public BannerController(ProniaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page=1)
        {
            var query = _context.Banners.AsQueryable();

            var data = PaginatedList<Banner>.Create(query, page, 2);

            if (data.TotalPage<page)
            {
                string url = Url.Action("index", "banner", new { page = data.TotalPage });
                return Redirect(url);              
            }
      
            return View(data);
        }
       

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Banner banner) 
        {
            if(!ModelState.IsValid) return View();

            if (banner.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View();
            }

            banner.ImageName = FileManager.Save(_env.WebRootPath, "uploads/banners", banner.ImageFile);

            _context.Banners.Add(banner);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Banner banner = _context.Banners.Find(id);

            if (banner == null) return View("Error");

            return View(banner);
        }

        [HttpPost]
        public IActionResult Edit(Banner banner)
        {
            if (!ModelState.IsValid) return View();

            Banner existBanner = _context.Banners.Find(banner.Id);

            if (existBanner == null) return View("Error");

            existBanner.Title = banner.Title;
            existBanner.Title2 = banner.Title2;
            existBanner.CollectionName = banner.CollectionName;
            existBanner.BtnUrl = banner.BtnUrl;
            existBanner.BtnText = banner.BtnText;

            string oldFileName = null;
            if (banner.ImageFile != null)
            {
                oldFileName = existBanner.ImageName;
                existBanner.ImageName = FileManager.Save(_env.WebRootPath, "uploads/banners", banner.ImageFile);
            }

            _context.SaveChanges();

            if (oldFileName != null)
                FileManager.Delete(_env.WebRootPath, "uploads/banners", oldFileName);

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Banner banner = _context.Banners.Find(id);

            if (banner == null) return StatusCode(404);
            FileManager.Delete(_env.WebRootPath, "uploads/banners", banner.ImageName);

            _context.Banners.Remove(banner);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
