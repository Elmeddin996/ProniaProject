using Microsoft.AspNetCore.Mvc;

namespace ProniaProject.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SliderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
