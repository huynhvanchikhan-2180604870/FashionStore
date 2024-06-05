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
            var productcount = _context.Products.Count();
            var usercount = _context.Users.Count();
            var orderscount = _context.Orders.Count();
            var turnover = _context.OrderDetails.Include(x => x.Product).Sum(x => x.Product.Price * x.Quantity);
            var model = new Dashbroad()
            {
                ProducstCount = productcount,
                UsersCount = usercount,
                OrdersCount = orderscount,
                Turnover = turnover
            };
            return View(model);
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
