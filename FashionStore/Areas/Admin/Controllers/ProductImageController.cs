using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductImageController : Controller
    {
        private readonly FashionStoreDbContext _dbContext;
        public ProductImageController(FashionStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Add(string id)
        {
            return View(new ProductImage { ProductId = id});
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductImage image)
        {
            if (ModelState.IsValid)
            {
                _dbContext.ProductImages.Add(image);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Products");
        }
    }
}
