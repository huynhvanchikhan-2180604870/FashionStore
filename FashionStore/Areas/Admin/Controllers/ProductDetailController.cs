using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductDetailController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FashionStoreDbContext _context;
        public ProductDetailController(FashionStoreDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Add(string id)
        {
            var sizes = await _context.Sizes.ToListAsync();
            var colors = await _context.Colors.ToListAsync();
            
            ViewBag.Sizes = new SelectList(sizes, "SizeID", "SizeName");
            ViewBag.Colors = new SelectList(colors, "ColorID", "ColorName");
            return View(new ProductDetail { ProductID = id});
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductDetail productDetail)
        {
            try
            {
                // Check if a ProductDetail with the same ProductID and SizeID already exists
                var existingDetail = await _context.ProductDetails
                    .FirstOrDefaultAsync(p => p.ProductID == productDetail.ProductID && p.SizeID == productDetail.SizeID);

                if (existingDetail != null)
                {
                    // If exists, update quantity
                    existingDetail.Quantity += productDetail.Quantity;
                }
                else
                {
                    // If not, create a new ProductDetail
                    _context.ProductDetails.Add(productDetail);
                }

                // Update Product's QuantityOnHand
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productDetail.ProductID);
                if (product != null)
                {
                    product.QuantityOnHand += productDetail.Quantity;
                    _context.Products.Update(product);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Products");
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ModelState.AddModelError("", "An error occurred while adding the product detail.");
                // Log the exception for troubleshooting
                _logger.LogError(ex, "Error occurred while adding product detail.");
                return RedirectToAction("Index", "Products");
            }
        }

    }
}
