using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace FashionStore.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly FashionStoreDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(FashionStoreDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var getUser = await _userManager.GetUserAsync(User);
            var getRole = await _userManager.GetRolesAsync(getUser);

            ViewBag.Role = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");
            var query = from user in _context.Users
                        join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
                        join roles in _context.Roles on userRoles.RoleId equals roles.Id
                        select new
                        {
                            UserId = user.Id,
                            user.UserName,
                            roles.Name,
                        };
            var list = await query.ToListAsync();
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            await _userManager.AddToRoleAsync(user, role.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
