using Azure;
using FashionStore.Data;
using FashionStore.Models;
using FashionStore.ModelsViews;
using FashionStore.ShoppingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace FashionStore.Controllers
{

    public class ProductController : Controller
    {
        private readonly FashionStoreDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _currentUser;
        public ProductController(FashionStoreDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 20;
            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .Where(p => p.QuantityOnHand > 0)
                .ToPagedListAsync(page, pagesize);
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

        public async Task<IActionResult> FilterByCategory(int? id, int page)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 20;
            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(x => x.CategoryId == id && x.QuantityOnHand > 0).ToPagedList(page, pagesize);
            var sizes = await _context.Sizes.Include(x => x.ProductDetails).ToListAsync();
            var brands = await _context.Brands.Include(x => x.Products).ToListAsync();
            var model = new MVShop
            {
                Categories = categories,
                Products = products,
                Sizes = sizes,
                Brands = brands
            };
            return View("Index", model);
        }

        public async Task<IActionResult> ProductDetail(string id)
        {
            var product = await _context.Products
                .Include(p => p.ProductDetails)
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(x => x.ProductID == id);
            var images = await _context.ProductImages
                .Include(p => p.Product)
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

        public async Task<IActionResult> Search(string? value, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 20;
            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .Where(p => p.QuantityOnHand > 0)
                .ToPagedListAsync(page, pagesize);
            var sizes = await _context.Sizes.Include(x => x.ProductDetails).ToListAsync();
            var brands = await _context.Brands.Include(x => x.Products).ToListAsync();

            var model = new MVShop
            {
                Categories = categories,
                Products = products,
                Sizes = sizes,
                Brands = brands
            };
            if (value != null)
            {
                var searchTerm = value.Trim().ToUpper(); // Loại bỏ khoảng trắng thừa và chuyển đổi giá trị nhập vào thành chuỗi tìm kiếm
                var filteredProducts = products.Where(x => x.ProductName.ToUpper().Contains(searchTerm)).ToPagedList(page, pagesize);
                model.Products = filteredProducts;
            }

            return View("Index", model);
        }

        public async Task<IActionResult> Sorting(int? typesoft, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 20;
            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .Where(p => p.QuantityOnHand > 0)
                .ToPagedListAsync(page, pagesize);
            var sizes = await _context.Sizes.Include(x => x.ProductDetails).ToListAsync();
            var brands = await _context.Brands.Include(x => x.Products).ToListAsync();

            var model = new MVShop
            {
                Categories = categories,
                Products = products,
                Sizes = sizes,
                Brands = brands
            };

            switch (typesoft)
            {
                case 1:
                    model.Products = model.Products.OrderBy(p => p.Price).ToPagedList(page, pagesize);
                    break;
                case 2:
                    model.Products = model.Products.Where(p => p.Price >= 0 && p.Price <= 55).ToPagedList(page, pagesize);
                    break;
                case 3:
                    model.Products = model.Products.Where(p => p.Price > 55 && p.Price <= 100).ToPagedList(page, pagesize);
                    break;
                default:
                    break;
            }
            return View("Index", model);
        }

        [Authorize]
        public async Task<IActionResult> Comment(string UserID, int Rating, string Comment, string ProductID)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == UserID);
            var comment = new Comment()
            {
                User = user,
                UserID = user.Id,
                FullName = user.FullName,
                ParentID = 0,
                Rate = Rating,
                CommentDate = DateTime.Now,
                ProductID = ProductID,
                Message = Comment
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> ProductEvent(int id, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 20;
            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();
            var events = await _context.Events
                .Include(p => p.ProductEvents)
                    .ThenInclude(p => p.Product)
                    .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(x => x.EventID == id);
            var productEvents = events.ProductEvents.ToPagedList(page, pagesize);
            // Khởi tạo danh sách sản phẩm
            List<Product> products = new List<Product>();

            // Lặp qua từng ProductEvent để lấy thông tin sản phẩm từ cơ sở dữ liệu
            foreach (var productEvent in productEvents)
            {
                var product = await _context.Products.FindAsync(productEvent.ProductID);
                if (product != null)
                {
                    products.Add(product);
                }
            }
            var sizes = await _context.Sizes.Include(x => x.ProductDetails).ToListAsync();
            var brands = await _context.Brands.Include(x => x.Products).ToListAsync();

            var model = new MVShop
            {
                Categories = categories,
                Products = products.ToPagedList(page,pagesize),
                Sizes = sizes,
                Brands = brands
            };
            return View("Index", model);
        }
    }
}
