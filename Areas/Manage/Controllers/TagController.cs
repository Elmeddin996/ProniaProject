using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProniaProject.Areas.Manage.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Models;
using System.Data;

namespace ProniaProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]

    [Area("manage")]
    public class TagController : Controller
    {
        private readonly ProniaContext _context;
        

        public TagController(ProniaContext context)
        {
            _context = context;
           
        }

        public IActionResult Index(int page=1, string search=null)
        {
            var query = _context.Tags.AsQueryable();

            if (search != null)
                query = query.Where(x => x.Name.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<Tag>.Create(query, page, 2));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Tag tag = _context.Tags.Find(id);

            if (tag == null) return StatusCode(404);

            return View(tag);
        }

        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            if (!ModelState.IsValid) return View();

            Tag existTag = _context.Tags.Find(tag.Id);

            if (existTag == null) return StatusCode(404);

            if (tag.Name != existTag.Name && _context.Tags.Any(x => x.Name == tag.Name))
            {
                ModelState.AddModelError("Name", "Name is already taken");
                return View();
            }

            existTag.Name = tag.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }


        public IActionResult Delete(int id)
        {
            Tag tag = _context.Tags.Find(id);

            if (tag == null) return StatusCode(404);

            _context.Tags.Remove(tag);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
