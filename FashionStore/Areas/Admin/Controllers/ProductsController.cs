using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var products = await _dbContext.Products
                .Include(cate=>cate.Category)
                .Include(material => material.Material)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Add()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            var materials = await _dbContext.Materials.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");
            ViewBag.Materials = new SelectList(materials, "MaterialID", "MaterialName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {

            if(ModelState.IsValid)
            {
                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult>Display(string id)
        {
            var details = await _dbContext.ProductDetails.FirstOrDefaultAsync(x => x.ProductID == id);
            if(details == null)
            {
                return RedirectToAction("Add","ProductDetail", new { id });
            }
            return View();
        }
    }
}
