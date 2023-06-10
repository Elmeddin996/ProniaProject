using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.Areas.Manage.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Models;
using System.Data;

namespace ProniaProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]

    [Area("manage")]
    public class CategorieController : Controller
    {
        private readonly ProniaContext _context;

        public CategorieController(ProniaContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page=1)
        {
            var query = _context.Categories.Include(x=>x.Plants).AsQueryable();

            return View(PaginatedList<Categorie>.Create(query,page,2));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categorie categorie)
        {
            if (!ModelState.IsValid)
                return View();

            if (_context.Categories.Any(x => x.Name == categorie.Name))
            {
                ModelState.AddModelError("Name", "Name is already taken");
                return View();
            }


            _context.Categories.Add(categorie);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Categorie categorie = _context.Categories.Find(id);

            if (categorie == null) return View("Error");

            return View(categorie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categorie categorie)
        {
            if (!ModelState.IsValid) return View();

            Categorie existCategorie = _context.Categories.Find(categorie.Id);

            if (existCategorie == null) return View("Error");

            if (categorie.Name != existCategorie.Name && _context.Categories.Any(x => x.Name == categorie.Name))
            {
                ModelState.AddModelError("Name", "Name is already taken");
                return View();
            }

            existCategorie.Name = categorie.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Categorie categorie = _context.Categories.Include(x => x.Plants).FirstOrDefault(x => x.Id == id);

            if (categorie == null) return View("Error");
            return View(categorie);
        }

        [HttpPost]
        public IActionResult Delete(Categorie categorie)
        {
            Categorie existCategorie = _context.Categories.Find(categorie.Id);

            if (existCategorie == null) return View("Error");

            _context.Categories.Remove(existCategorie);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


    }
}
