using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Areas.Admin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InvoicesController : ControllerBase
	{
		private readonly FashionStoreDbContext _context;
		public InvoicesController(FashionStoreDbContext context)
		{
			_context = context;
		}
		[HttpGet("date/{orderDate}")]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByDate(DateTime orderDate)
		{
			// Normalize the date to remove the time part
			var normalizedOrderDate = orderDate.Date;

			var orders = await _context.Orders
				.Include(o => o.Details)
					.ThenInclude(d => d.Product)
						.ThenInclude(p => p.Category)
				.Where(o => o.OrderDate.Date == normalizedOrderDate)
				.ToListAsync();

			if (orders == null || orders.Count == 0)
			{
				return NotFound();
			}

			return Ok(orders);
		}

		[HttpGet("orders")]
		public async Task<ActionResult<IEnumerable<OrderSummary>>> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
		{
			var orders = await _context.Orders
				.Include(o => o.Details)
				.ThenInclude(d => d.Product)
				.Where(o => o.OrderDate.Date >= startDate.Date && o.OrderDate.Date <= endDate.Date)
				.ToListAsync();

			var orderSummary = orders
				.SelectMany(o => o.Details)
				.GroupBy(d => d.Product.ProductName)
				.Select(g => new OrderSummary { ProductName = g.Key, Quantity = g.Sum(d => d.Quantity) })
				.ToList();

			return Ok(orderSummary);
		}

		public class OrderSummary
		{
			public string ProductName { get; set; }
			public int Quantity { get; set; }
		}

	}
    public class OrderSummary
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
