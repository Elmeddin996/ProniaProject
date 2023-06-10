using Microsoft.AspNetCore.Mvc;

namespace ProniaProject.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
