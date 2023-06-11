using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.DAL;
using System.Data;

namespace ProniaProject.Areas.Manage.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]

    [Area("manage")]
    public class DashboardController : Controller
    {
        private readonly ProniaContext _context;

        public DashboardController(ProniaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult getPieChartDatas()
        {
            var categorieNames = _context.Categories.Select(x => x.Name).ToList();
            var categoriePlantCount = _context.Categories.Include(x => x.Plants).Select(x => x.Plants.Count).ToList();
            List<string> colors = new List<string>();

            for (int i = 0; i < categorieNames.Count; i++)
            {
                Random random = new Random();
                string randomColor = string.Format("#{0:X6}", random.Next(0x1000000));
                colors.Add(randomColor);
            }

            return Json(new
            {
                CategorieNames = categorieNames,
                CategoriePlantCount = categoriePlantCount,
                Colors = colors
            });
        }

        public IActionResult getLinearChartDatas()
        {


            return Json(new
            {

                Labels = new string[] { "Yanvdsar", "Fevral", "Mart" }
            });

        }
    }
}