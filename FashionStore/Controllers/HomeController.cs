using FashionStore.Data;
using FashionStore.Models;
using FashionStore.ModelsViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FashionStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FashionStoreDbContext _context;
        public HomeController(ILogger<HomeController> logger, FashionStoreDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productsWithEvents = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.ProductDetails)
                .Include(p => p.ProductEvents)
                    .ThenInclude(pe => pe.Event)
                .Where(p => p.ProductEvents.Any())
                .ToListAsync();
            var events = await _context.Events
                .Include(p=> p.Banners)
                .Include(p=> p.ProductEvents)
                    .ThenInclude(p => p.Product)
                    .ThenInclude(p => p.Images)
                .Where(p => p.EndDate >= DateTime.Now)
                .ToListAsync();

            // Ghi log dữ liệu để kiểm tra
            foreach (var ev in events)
            {
                _logger.LogInformation($"Event: {ev.EventName}");
                foreach (var banner in ev.Banners)
                {
                    _logger.LogInformation($"Banner URL: {banner.UrlImage}");
                }
            }

            var model = new MVProductEvent()
            {
                Products = productsWithEvents,
                Events = events
            };
            return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
