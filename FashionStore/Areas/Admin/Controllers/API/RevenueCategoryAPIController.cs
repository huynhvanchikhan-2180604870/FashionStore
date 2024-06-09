using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FashionStore.Models;
using FashionStore.Data;
using FashionStore.Areas.Admin.Models;

namespace FashionStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueCategoryAPIController : ControllerBase
    {
        private readonly FashionStoreDbContext _context;

        public RevenueCategoryAPIController(FashionStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("order-count")]
        public async Task<ActionResult<IEnumerable<CategoryOrderCount>>> GetOrderCountByCategory()
        {
            var orderCounts = await _context.Categories
                .Select(category => new CategoryOrderCount
                {
                    CategoryName = category.CategoryName,
                    OrderCount = _context.Orders
                        .Where(order => order.Details.Any(detail => detail.Product.CategoryId == category.CategoryID))
                        .Count()
                })
                .ToListAsync();

            return orderCounts;
        }

    }
}
