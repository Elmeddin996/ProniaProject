using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProniaProject.Areas.Manage.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.ViewModel;

namespace ProniaProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProniaContext _context;

        public ShopController(ProniaContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? categorieId, double? minPrice = null, double? maxPrice = null, string sort = "AToZ", int page=1)
        {
            var query = _context.Plants.Include(x => x.PlantImages.Where(x => x.PosterStatus != null)).AsQueryable();
            
            ShopViewModel shopVM = new ShopViewModel
            {
                Categories = _context.Categories.Include(x => x.Plants).ToList(),
                Tags = _context.Tags.ToList(),
                PaginatedList = PaginatedList<Plant>.Create(query, page, 3)
            };



            if (categorieId != null)
                query = query.Where(x => x.CategorieId == categorieId);

            if (minPrice != null && maxPrice != null)
                query = query.Where(x => x.SalePrice >= (decimal)minPrice && x.SalePrice <= (decimal)maxPrice);

            switch (sort)
            {
                case "AToZ":
                    query = query.OrderBy(x => x.Name);
                    break;
                case "ZToA":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case "LowToHigh":
                    query = query.OrderBy(x => x.SalePrice);
                    break;
                case "HighToLow":
                    query = query.OrderByDescending(x => x.SalePrice);
                    break;
            }


            shopVM.Plants = query.ToList();

            ViewBag.MaxPriceLimit = _context.Plants.Max(x => x.SalePrice);

            ViewBag.SortList = new List<SelectListItem>
            {
                new SelectListItem {Value="AToZ",Text= "Sort By:Name (A - Z)",Selected=sort=="AToZ"},
                new SelectListItem { Value = "ZToA", Text = "Sort By:Name (Z - A)", Selected = sort == "ZToA" },
                new SelectListItem { Value = "LowToHigh", Text = "Sort By:Name (Low - High)", Selected = sort == "LowToHigh" },
                new SelectListItem { Value = "HighToLow", Text = "Sort By:Name (High - Low)", Selected = sort == "HighToLow" }
            };


            ViewBag.Sort = sort;
            ViewBag.GenreId = categorieId;
            ViewBag.MinPrice = minPrice ?? 0;
            ViewBag.MaxPrice = maxPrice ?? ViewBag.MaxPriceLimit;


            return View(shopVM);
        }
    }
}
