﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.DAL;
using ProniaProject.ViewModel;
using System.Diagnostics;

namespace ProniaProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProniaContext _context;

        public HomeController(ProniaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel
            {
                Bestsellers= _context.Plants.Include(x=>x.PlantImages).Where(x=>x.Bestseller).Take(10).ToList(),
                Features = _context.Plants.Include(x => x.PlantImages).Where(x => x.IsFeatured).Take(10).ToList(),
                New = _context.Plants.Include(x => x.PlantImages).Where(x => x.IsNew).Take(10).ToList(),
                Sliders=_context.Sliders.ToList()
            };
            return View(vm);
        }
    }
}