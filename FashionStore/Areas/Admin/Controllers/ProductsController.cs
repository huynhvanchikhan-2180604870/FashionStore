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
            var details = await _dbContext.ProductDetails
                .Include(p => p.Size)
                .Include(x => x.Product)
                .Where(x => x.ProductID == id)  // Filter based on provided id
                .ToListAsync();

            if (details.Count <= 0)
            {
                return RedirectToAction("Add","ProductDetail", new { id });
            }
            return View(details);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var productDetail = await _dbContext.ProductDetails
                .Include(p => p.Product)
                .Include(s => s.Size)
                .Where(x => x.ProductID == id)
                .ToListAsync();

            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == id);
            if(product != null)
            {
                product.ProductDetails = productDetail;
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletedConfirm(string id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == id);
            if (product != null)
            {
                var productdetails = await _dbContext.ProductDetails.Where(x => x.ProductID == product.ProductID).ToListAsync();

                for (int i = productdetails.Count - 1; i >= 0; i--)
                {
                    var detail = productdetails[i];
                    _dbContext.ProductDetails.Remove(detail);
                }

                await _dbContext.SaveChangesAsync();

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


    }
}
