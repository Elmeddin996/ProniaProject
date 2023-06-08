using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.ViewModel;

namespace ProniaProject.Controllers
{
    public class PlantController : Controller
    {
        private readonly ProniaContext _context;

        public PlantController(ProniaContext context)
        {
            _context = context;
        }
        public IActionResult Detail(int id)
        {
            Plant plant = _context.Plants
                .Include(x => x.PlantImages)
                .Include(x => x.Categories)
                .Include(x => x.PlantComments).ThenInclude(x => x.AppUser)
                .Include(x => x.PlantTags).ThenInclude(bt => bt.Tag).FirstOrDefault(x => x.Id == id);

            if (plant == null) return View("Error");

            PlantDetailViewModel vm = new PlantDetailViewModel
            {
                Plant = plant,
                RelatedPlants = _context.Plants.Include(x => x.PlantImages).Where(x => x.CategorieId == plant.CategorieId).ToList(),
                Comment = new PlantComment { PlantId = id }
            };

            return View(vm);
        }

        public IActionResult GetPlantDetail(int id)
        {
            Plant plant = _context.Plants
                .Include(x => x.PlantImages)
                .Include(x => x.PlantTags).ThenInclude(x => x.Tag)
                .FirstOrDefault(x => x.Id == id);

            if (plant == null) return View("Error");

            return PartialView("_PlantModalPartial", plant);
        }


    }
}
