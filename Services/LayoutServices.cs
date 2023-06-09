namespace ProniaProject.Services
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using ProniaProject.DAL;
    using ProniaProject.Models;
    using ProniaProject.ViewModel;
    using System.Security.Claims;



    public class LayoutServices
    {
        private readonly ProniaContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutServices(ProniaContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }

        public List<Categorie> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public BasketViewModel GetBasket()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && _httpContextAccessor.HttpContext.User.IsInRole("Member"))
            {
                var basketItems = _context.BasketItems.Include(x => x.Plant).ThenInclude(x => x.PlantImages).Where(x => x.AppUserId == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
                var bv = new BasketViewModel();
                foreach (var ci in basketItems)
                {
                    BasketItemViewModel bi = new BasketItemViewModel
                    {
                        Count = ci.Count,
                        Plant = ci.Plant
                    };
                    bv.BasketItems.Add(bi);
                    bv.TotalPrice += bi.Plant.SalePrice * bi.Count;
                }
                return bv;
            }
            else
            {
                var bv = new BasketViewModel();
                var basketJson = _httpContextAccessor.HttpContext.Request.Cookies["basket"];

                if (basketJson != null)
                {
                    var cookieItems = JsonConvert.DeserializeObject<List<BasketItemCookieViewModel>>(basketJson);

                    foreach (var ci in cookieItems)
                    {
                        BasketItemViewModel bi = new BasketItemViewModel
                        {
                            Count = ci.Count,
                            Plant = _context.Plants.Include(x => x.PlantImages).FirstOrDefault(x => x.Id == ci.PlantId)
                        };
                        bv.BasketItems.Add(bi);
                        bv.TotalPrice +=  bi.Plant.SalePrice * bi.Count;
                    }
                }

                return bv;
            }

        }
    }
}
