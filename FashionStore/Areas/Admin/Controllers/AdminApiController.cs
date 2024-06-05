using FashionStore.Areas.Admin.Models;
using FashionStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FashionStore.Areas.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminApiController : ControllerBase
    {
        private readonly FashionStoreDbContext _context;

        public AdminApiController(FashionStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("chartdata")]
        public IActionResult GetChartData()
        {
            var categories = new List<string> { "Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy" };
            var ordersData = new int[7];
            var revenueData = new int[7];

            var orders = _context.Orders.Include(o => o.Details).ToList();

            foreach (var order in orders)
            {
                var dayOfWeek = (int)order.OrderDate.DayOfWeek;
                ordersData[dayOfWeek]++;
                revenueData[dayOfWeek] += (int)order.Details.Sum(od => od.Price * od.Quantity);
            }

            var series = new List<ChartData>
        {
            new ChartData
            {
                Name = "Đơn hàng",
                Data = ordersData.ToList()
            },
            new ChartData
            {
                Name = "Doanh thu",
                Data = revenueData.ToList()
            }
        };

            var response = new ChartResponse
            {
                Categories = categories,
                Series = series
            };

            return Ok(response);
        }
    }
}