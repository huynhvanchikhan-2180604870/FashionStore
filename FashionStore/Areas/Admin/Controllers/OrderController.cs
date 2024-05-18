using FashionStore.Data;
using FashionStore.HelperClass;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using X.PagedList;
using X.PagedList.Mvc;
using static NuGet.Packaging.PackagingConstants;

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

		public async Task<IActionResult> Index(int page = 1)
		{
			page = page < 1 ? 1 : page;
			int pagesize = 10;
			var orders = await _context.Orders
				.Include(o => o.User)
				.Include(o => o.Details)
					.ThenInclude(os => os.Product)
					.ThenInclude(os => os.Images)
				.ToPagedListAsync(page, pagesize);
			return View(orders);
		}

		public async Task<IActionResult> UpdateStatus(int id)
		{
			var order = await _context.Orders
				.Include(o => o.User)
				.Include(o => o.Details)
					.ThenInclude(os => os.Product)
					.ThenInclude(os => os.Images)
				.FirstOrDefaultAsync(x => x.OrderID==id);
            var orderStatusList = new List<SelectListItem>
			{
				new SelectListItem { Value = OrderStatus.CANCELLED.ToString(), Text = "Cancelled" },
				new SelectListItem { Value = OrderStatus.PROCESSING.ToString(), Text = "Processing" },
				new SelectListItem { Value = OrderStatus.DELIVERED.ToString(), Text = "Delivered" }
			};
			ViewBag.Status = orderStatusList;
            return View(order);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateStatus(Order order)
		{
			var currentOrder = await _context.Orders
				.Include(o => o.User)
				.Include(o => o.Details)
				.FirstOrDefaultAsync(x => x.OrderID == order.OrderID);

			currentOrder.Status = order.Status;
			_context.Orders.Update(currentOrder);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		
	}
}
