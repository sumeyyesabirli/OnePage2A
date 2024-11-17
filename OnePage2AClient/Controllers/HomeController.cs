using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePage2AClient.Models;
using OnePage2ADataAccess.Contexts;

namespace OnePage2AClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbContext2A _context;
        public HomeController(DbContext2A context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var homeViewModel = new HomeViewModel
            {
                Banner = await _context.Banners.ToListAsync(),
                AboutUs = await _context.AboutUs.FirstOrDefaultAsync(),
                Services = await _context.Services.ToListAsync(),
                References = await _context.References.ToListAsync(),
                Gallery = await _context.Galleries.ToListAsync(),
                Contact = await _context.Contacts.FirstOrDefaultAsync()
            };

            return View(homeViewModel);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}
