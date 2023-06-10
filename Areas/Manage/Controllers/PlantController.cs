using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.Areas.Manage.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Helpers;
using ProniaProject.Models;
using System.Data;
using System.Numerics;

namespace ProniaProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]

    [Area("manage")]
    public class PlantController : Controller
    {
        private readonly ProniaContext _context;

        private readonly IWebHostEnvironment _env;

        public PlantController(ProniaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1, string search = null)
        {
            var query = _context.Plants
                .Include(x => x.Categories).Include(x => x.PlantImages.Where(pi => pi.PosterStatus == true)).AsQueryable();

            if (search != null)
                query = query.Where(x => x.Name.Contains(search));

            ViewBag.Search = search;

            return View(PaginatedList<Plant>.Create(query, page, 3));
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Plant plant)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View();
            }

            if (!_context.Categories.Any(x => x.Id == plant.CategorieId))
            {
                ModelState.AddModelError("CategorieId", "Categorie is not correct");
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View();
            }


            if (plant.PosterImage == null)
            {
                ModelState.AddModelError("PosterImage", "PosterImage is required");
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View();
            }
            if (plant.HoverPosterImage == null)
            {
                ModelState.AddModelError("HoverPosterImage", "Hover Poster Image is required");
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View();
            }

            foreach (var tagId in plant.TagIds)
            {
                PlantTag plantTag = new PlantTag
                {
                    TagId = tagId,
                };

                plant.PlantTags.Add(plantTag);
            }

            PlantImage poster = new PlantImage
            {
                ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.PosterImage),
                PosterStatus = true,
            };
            plant.PlantImages.Add(poster);

            PlantImage hoverPoster = new PlantImage
            {
                ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.HoverPosterImage),
                PosterStatus = false,
            };
            plant.PlantImages.Add(hoverPoster);

            foreach (var img in plant.Images)
            {
                PlantImage plantImage = new PlantImage
                {
                    ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", img),
                };
                plant.PlantImages.Add(plantImage);
            }



            _context.Plants.Add(plant);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();


            Plant plant = _context.Plants.Include(x => x.PlantImages).Include(x => x.PlantTags).FirstOrDefault(x => x.Id == id);

            plant.TagIds = plant.PlantTags.Select(x => x.TagId).ToList();


            return View(plant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Plant plant)
        {
            if (!ModelState.IsValid) return View();

            Plant existPlant = _context.Plants.Include(x => x.PlantTags).Include(x => x.PlantImages).FirstOrDefault(x => x.Id == plant.Id);

            if (existPlant == null) return View("Error");

            if (plant.CategorieId != existPlant.CategorieId && !_context.Categories.Any(x => x.Id == plant.CategorieId))
            {
                ModelState.AddModelError("CategorieId", "Categorie is not correct");
                return View();
            }

            existPlant.PlantTags.RemoveAll(x => !plant.TagIds.Contains(x.TagId));

            var newTagIds = plant.TagIds.FindAll(x => !existPlant.PlantTags.Any(bt => bt.TagId == x));
            foreach (var tagId in newTagIds)
            {
                PlantTag plantTag = new PlantTag { TagId = tagId };
                existPlant.PlantTags.Add(plantTag);
            }


            string oldPoster = null;
            if (plant.PosterImage != null)
            {
                PlantImage poster = existPlant.PlantImages.FirstOrDefault(x => x.PosterStatus == true);
                oldPoster = poster?.ImageName;

                if (poster == null)
                {
                    poster = new PlantImage { PosterStatus = true };
                    poster.ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.PosterImage);
                    existPlant.PlantImages.Add(poster);
                }
                else
                    poster.ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.PosterImage);
            }

            string oldHoverPoster = null;
            if (plant.HoverPosterImage != null)
            {
                PlantImage hoverPoster = existPlant.PlantImages.FirstOrDefault(x => x.PosterStatus == false);
                oldHoverPoster = hoverPoster?.ImageName;

                if (hoverPoster == null)
                {
                    hoverPoster = new PlantImage { PosterStatus = false };
                    hoverPoster.ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.HoverPosterImage);
                    existPlant.PlantImages.Add(hoverPoster);
                }
                else
                    hoverPoster.ImageName = FileManager.Save(_env.WebRootPath, "uploads/plants", plant.HoverPosterImage);
            }

            var removedImages = existPlant.PlantImages.FindAll(x => x.PosterStatus == null && !plant.PlantImageIds.Contains(x.Id));
            existPlant.PlantImages.RemoveAll(x => x.PosterStatus == null && !plant.PlantImageIds.Contains(x.Id));

            foreach (var item in plant.Images)
            {
                PlantImage plantImage = new PlantImage
                {
                    ImageName = FileManager.Save(_env.WebRootPath, "uploads/plant", item),
                };
                existPlant.PlantImages.Add(plantImage);
            }

            existPlant.Name = plant.Name;
            existPlant.SalePrice = plant.SalePrice;
            existPlant.CostPrice = plant.CostPrice;
            existPlant.Desc = plant.Desc;
            existPlant.IsFeatured = plant.IsFeatured;
            existPlant.IsNew = plant.IsNew;
            existPlant.Bestseller = plant.Bestseller;
            existPlant.CategorieId = plant.CategorieId;

            _context.SaveChanges();


            if (oldPoster != null) FileManager.Delete(_env.WebRootPath, "uploads/plants", oldPoster);
            if (oldHoverPoster != null) FileManager.Delete(_env.WebRootPath, "uploads/plants", oldHoverPoster);

            if (removedImages.Any())
                FileManager.DeleteAll(_env.WebRootPath, "uploads/plants", removedImages.Select(x => x.ImageName).ToList());


            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Plant plant = _context.Plants.Include(x => x.PlantImages).FirstOrDefault(x => x.Id == id);
            if (plant == null) return View("Error");
            return View(plant);
        }

        [HttpPost]
        public IActionResult Delete(Plant plant)
        {
            Plant existPlant = _context.Plants.Include(x => x.PlantImages).FirstOrDefault(x => x.Id == plant.Id);

            if (existPlant == null) return View("error");

            var removedImages = existPlant.PlantImages;

            if (removedImages.Any())
                FileManager.DeleteAll(_env.WebRootPath, "uploads/plants", removedImages.Select(x => x.ImageName).ToList());


            _context.Plants.Remove(existPlant);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
