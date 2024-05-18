using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Areas.Admin.Controllers
{
	[Authorize(Roles =SD.Role_Admin)]
	[Area("Admin")]
	public class OrderController : Controller
	{
		private readonly FashionStoreDbContext _context;
		public OrderController(FashionStoreDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var orders = await _context.Orders
				.Include(o => o.User)
				.Include(o => o.Details)
					.ThenInclude(os => os.Product)
					.ThenInclude(os => os.Images)
				.ToListAsync();
			return View(orders);
		}
	}
}
