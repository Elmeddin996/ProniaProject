using Microsoft.AspNetCore.Mvc;
using ProniaProject.DAL;

namespace ProniaProject.Areas.Manage.Controllers
{
    [Area("manage")]
    public class TagController : Controller
    {
        private readonly ProniaContext _context;
        

        public TagController(ProniaContext context)
        {
            _context = context;
           
        }

        public IActionResult Index(int page=1)
        {

            return View();
        }


    }
}
