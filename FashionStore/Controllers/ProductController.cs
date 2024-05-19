using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FashionStore.Models;
using FashionStore.HelperClass;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using System.Text.Json;
using FashionStore.Data;
using Microsoft.AspNetCore.Identity;
using FashionStore.ModelsViews;
using FashionStore.ShoppingModels;
using Microsoft.AspNetCore.Authorization;

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
            int pageSize = 20;

            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();

            var productsFromSession = HttpContext.Session.GetObjectFromJson<List<Product>>("ProductFilter");
            int? categoryId = Microsoft.AspNetCore.Http.SessionExtensions.GetInt32(HttpContext.Session, "SelectedCategoryId");

            IPagedList<Product> products;
            if (productsFromSession == null || categoryId == null)
            {
                products = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Material)
                    .Include(p => p.Images)
                    .Include(p => p.Category)
                    .ToPagedListAsync(page, pageSize);
            }
            else
            {
                var productsWithImages = new List<Product>();

                foreach (var product in productsFromSession)
                {
                    var images = await _context.ProductImages.Where(i => i.ProductId == product.ProductID).ToListAsync();
                    product.Images = images;
                    productsWithImages.Add(product);
                }

                products = productsWithImages
                    .Where(p => p.CategoryId == categoryId)
                    .ToPagedList(page, pageSize);
            }

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


        public async Task<IActionResult> FilterByCategory(int? id, int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pageSize = 20;

            // Store the selected category ID in session
            Microsoft.AspNetCore.Http.SessionExtensions.SetInt32(HttpContext.Session, "SelectedCategoryId", id.HasValue ? id.Value : 0);

            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();

            List<Product> productList;
            if (id.HasValue)
            {
                productList = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Material)
                    .Include(p => p.Images)
                    .Include(p => p.Category)
                    .Where(x => x.CategoryId == id && x.QuantityOnHand > 0)
                    .ToListAsync();
            }
            else
            {
                productList = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Material)
                    .Include(p => p.Images)
                    .Include(p => p.Category)
                    .ToListAsync();
            }

            // Store filtered products in session
            HttpContext.Session.SetObjectAsJson("ProductFilter", productList);

            var products = productList.ToPagedList(page, pageSize);

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
            int pageSize = 20;

            // Store the selected sorting type in session
            if (typesoft.HasValue)
            {
                Microsoft.AspNetCore.Http.SessionExtensions.SetInt32(HttpContext.Session, "SelectedSortingType", typesoft.Value);
            }
            else
            {
                // Retrieve the sorting type from session if not provided
                typesoft = FashionStore.HelperClass.SessionExtensions.GetInt32(HttpContext.Session, "SelectedSortingType");
            }

            var categories = await _context.Categories.Include(x => x.Products).ToListAsync();
            var productsQuery = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Material)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .Where(p => p.QuantityOnHand > 0);

            // Apply sorting based on the selected type
            switch (typesoft)
            {
                case 1:
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
                case 2:
                    productsQuery = productsQuery.Where(p => p.Price >= 0 && p.Price <= 500000);
                    break;
                case 3:
                    productsQuery = productsQuery.Where(p => p.Price > 500000 && p.Price <= 15000000);
                    break;
                default:
                    break;
            }

            var products = await productsQuery.ToPagedListAsync(page, pageSize);
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
