using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.ViewModel;
using System.Security.Claims;

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


        public IActionResult AddToCart(int id)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var basketItem = _context.BasketItems.FirstOrDefault(x => x.PlantId == id && x.AppUserId == userId);
                if (basketItem != null) basketItem.Count++;

                else
                {
                    basketItem = new BasketItem { AppUserId = userId, PlantId = id, Count = 1 };
                    _context.BasketItems.Add(basketItem);
                }
                _context.SaveChanges();
                var basketItems = _context.BasketItems.Include(x => x.Plant).ThenInclude(x => x.PlantImages).Where(x => x.AppUserId == userId).ToList();


                return PartialView("_CartPartialView", GenerateBasketVM(basketItems));
            }
            else
            {
                List<BasketItemCookieViewModel> cookieItems = new List<BasketItemCookieViewModel>();

                BasketItemCookieViewModel cookieItem;
                var basketStr = Request.Cookies["basket"];
                if (basketStr != null)
                {
                    cookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);

                    cookieItem = cookieItems.FirstOrDefault(x => x.PlantId == id);

                    if (cookieItem != null)
                        cookieItem.Count++;
                    else
                    {
                        cookieItem = new BasketItemCookieViewModel { PlantId = id, Count = 1 };
                        cookieItems.Add(cookieItem);
                    }
                }
                else
                {
                    cookieItem = new BasketItemCookieViewModel { PlantId = id, Count = 1 };
                    cookieItems.Add(cookieItem);
                }

                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));
                return PartialView("_CartPartialView", GenerateBasketVM(cookieItems));
            }
        }

        private BasketViewModel GenerateBasketVM(List<BasketItemCookieViewModel> cookieItems)
        {
            BasketViewModel bv = new BasketViewModel();
            foreach (var ci in cookieItems)
            {
                BasketItemViewModel bi = new BasketItemViewModel
                {
                    Count = ci.Count,
                    Plant = _context.Plants.Include(x => x.PlantImages).FirstOrDefault(x => x.Id == ci.PlantId)
                };
                bv.BasketItems.Add(bi);
                bv.TotalPrice += bi.Plant.SalePrice * bi.Count;
            }

            return bv;
        }

        private BasketViewModel GenerateBasketVM(List<BasketItem> basketItems)
        {
            BasketViewModel bv = new BasketViewModel();
            foreach (var item in basketItems)
            {
                BasketItemViewModel bi = new BasketItemViewModel
                {
                    Count = item.Count,
                    Plant = item.Plant
                };
                bv.BasketItems.Add(bi);
                bv.TotalPrice += bi.Plant.SalePrice * bi.Count;
            }
            return bv;
        }


        public IActionResult RemoveBasket(int id)
        {
            var basketStr = Request.Cookies["Basket"];
            if (basketStr == null)
                return StatusCode(404);

            List<BasketItemCookieViewModel> cookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketStr);

            BasketItemCookieViewModel item = cookieItems.FirstOrDefault(x => x.PlantId == id);

            if (item == null)
                return StatusCode(404);

           
            cookieItems.Remove(item);

            Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));

            BasketViewModel bv = new BasketViewModel();
            foreach (var ci in cookieItems)
            {
                BasketItemViewModel bi = new BasketItemViewModel
                {
                    Count = ci.Count,
                    Plant = _context.Plants.Include(x => x.PlantImages).FirstOrDefault(x => x.Id == ci.PlantId)
                };
                bv.BasketItems.Add(bi);
                bv.TotalPrice += bi.Plant.SalePrice * bi.Count;
            }

            return PartialView("_CartPartialView", bv);
        }

      

        public IActionResult ViewCart()
        {
            BasketViewModel bv = new BasketViewModel();
            var basketItems = _context.BasketItems.Include(x=>x.Plant).ThenInclude(x=>x.PlantImages);
            foreach (var item in basketItems)
            {
                BasketItemViewModel bi = new BasketItemViewModel
                {
                    Plant =  item.Plant,
                    Count = item.Count
                };
                bv.BasketItems.Add(bi);
            }
            return View(bv);
        }
    }
}
    
