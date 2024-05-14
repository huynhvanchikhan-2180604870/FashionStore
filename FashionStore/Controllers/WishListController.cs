using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Controllers
{
    [Authorize]
    public class WishListController : Controller
    {
        private readonly FashionStoreDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishListController(FashionStoreDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var wishlist = await _context.WishList
                .Include(x => x.Product) // Bao gồm thông tin về sản phẩm
                    .ThenInclude(p => p.Images) // Bao gồm thông tin về hình ảnh của sản phẩm
                .Where(x => x.UserID == user.Id)
                .ToListAsync();

            return View(wishlist);
        }

        public async Task<IActionResult>AddWishList(string productid)
        {
            var user = await _userManager.GetUserAsync(User);
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == productid);

                var wishlist = new WishList()
                {
                    User = user,
                    UserID = user.Id,
                    ProductID = product.ProductID,
                    Product = product
                };
                _context.WishList.Add(wishlist);
                await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveItem(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var wishlist = await _context.WishList
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.ProductID == id && x.UserID == user.Id);
            _context.WishList.Remove(wishlist);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
