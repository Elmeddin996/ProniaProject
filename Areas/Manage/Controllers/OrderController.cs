using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.Areas.Manage.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Enums;
using ProniaProject.Models;
using ProniaProject.Services;

namespace ProniaProject.Areas.Manage.Controllers
{

    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class OrderController : Controller
    {
        private readonly ProniaContext _context;
        private readonly IEmailSender _emailSender;

        public OrderController(ProniaContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Orders.Include(x => x.OrderItems).AsQueryable();
            var data = PaginatedList<Order>.Create(query, page, 3);

            return View(data);
        }


        public IActionResult Detail(int id)
        {
            Order order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Plant).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return View("Error");

            return View(order);
        }

        public async Task<IActionResult> Accept(int id)
        {
            Order order = _context.Orders.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return View("Error");

            order.Status = OrderStatus.Accepted;
            _context.SaveChanges();

            _emailSender.Send(order.Email, "Your order accepted", "Your order accepted!");

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Reject(int id)
        {
            Order order = _context.Orders.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id);

            if (order == null)
                return View("Error");

            order.Status = OrderStatus.Rejected;
            _context.SaveChanges();
            _emailSender.Send(order.Email, "Your order rejected", "Your order rejected!");

            
            return RedirectToAction("index");
        }
    }
}
