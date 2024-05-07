using FashionStore.Data;
using FashionStore.ModelsViews;
using FashionStore.ShoppingModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Controllers
{

    public class ProductController : Controller
    {
        private readonly FashionStoreDbContext _context;
        public ProductController(FashionStoreDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(p => p.QuantityOnHand > 0)
                .ToListAsync();
            var sizes = await _context.Sizes.Include(x => x.ProductDetails).ToListAsync();
            var brands = await _context.Brands.Include(x => x.Products).ToListAsync();

            var model = new MVShop
            {
                Categories = categories,
                Products = products,
                Sizes = sizes,
                Brands = brands
            };

            return View(model);
        }

        public async Task<IActionResult> FilterByCategory(int? id)
        {
            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(x => x.CategoryId == id && x.QuantityOnHand > 0);
            var sizes = await _context.Sizes.Include(x => x.ProductDetails).ToListAsync();
            var brands = await _context.Brands.Include(x => x.Products).ToListAsync();
            var model = new MVShop
            {
                Categories = categories,
                Products = products,
                Sizes = sizes,
                Brands = brands
            };
            return View("Index",model);
        }

        public async Task<IActionResult> ProductDetail(string id)
        {
            var product = await _context.Products
                .Include(p => p.ProductDetails)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(x => x.ProductID == id);
            var images = await _context.ProductImages
                .Include(p=> p.Product)
                .Where(x => x.ProductId == id).ToListAsync();
            var productdetails = await _context.ProductDetails
                .Include(x => x.Size)
                .Include(x => x.Product)
                .Where(x => x.ProductID == id).ToListAsync();
            var model = new CartItem()
            {
                Product = product,
                ProductDetails = productdetails,
                Images = images,
                ProductId = id
            };
            return View(model);
        }

    }
}
