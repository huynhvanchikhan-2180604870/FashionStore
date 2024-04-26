using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductsController : Controller
    {
        private readonly FashionStoreDbContext _dbContext;
        public ProductsController(FashionStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products.ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Add()
        {
            //ViewBag.Categories = new Lis
            return View();
        }
    }
}
