using FashionStore.Areas.Admin.Models;
using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class HomeController : Controller
    {
        private readonly FashionStoreDbContext _context;
        public HomeController(FashionStoreDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }

        public IActionResult RevenueCategory()
        {
            return View();
        }
    }
}
