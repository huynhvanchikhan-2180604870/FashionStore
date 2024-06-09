using FashionStore.Data;
using FashionStore.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Import thư viện logging
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FashionStore.Areas.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminApiController : ControllerBase
    {
        private readonly FashionStoreDbContext _context;
        private readonly ILogger<AdminApiController> _logger; // Thêm logger

        public AdminApiController(FashionStoreDbContext context, ILogger<AdminApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("chartdata")]
        public async Task<IActionResult> GetChartData(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                return BadRequest("Ngày kết thúc phải lớn hơn ngày bắt đầu.");
            }

            try
            {
                var totalDays = (endDate - startDate).Days;
                if (totalDays > 150)
                {
                    return await GetMonthlyData(startDate, endDate);
                }
                else if (totalDays > 30)
                {
                    return await GetWeeklyData(startDate, endDate);
                }
                else if (totalDays >= 7)
                {
                    return await GetWeeklyData(startDate, endDate);
                }
                else
                {
                    return await GetDailyData(startDate, endDate);
                }
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về lỗi 500
                _logger.LogError(ex, "An error occurred while getting chart data.");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        private async Task<IActionResult> GetMonthlyData(DateTime startDate, DateTime endDate)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Details)
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                    .ToListAsync();

                var monthlyData = orders
                    .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                    .Select(g => new
                    {
                        Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                        OrdersCount = g.Count(),
                        Revenue = g.Sum(o => o.Details.Sum(d => d.Price * d.Quantity))
                    })
                    .ToList();

                var result = monthlyData.Select(md => new
                {
                    md.Month,
                    md.OrdersCount,
                    Revenue = (int)md.Revenue
                }).ToList();

                return Ok(new
                {
                    categories = result.Select(m => m.Month).ToArray(),
                    series = new[]
                    {
                        new { name = "Đơn hàng", data = result.Select(m => m.OrdersCount).ToArray() },
                        new { name = "Doanh thu", data = result.Select(m => m.Revenue).ToArray() }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting monthly data.");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        private async Task<IActionResult> GetWeeklyData(DateTime startDate, DateTime endDate)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Details)
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                    .ToListAsync();

                var weeklyData = orders
                    .GroupBy(o => System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(o.OrderDate, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                    .Select(g => new
                    {
                        Week = $"Tuần {g.Key}",
                        OrdersCount = g.Count(),
                        Revenue = g.Sum(o => o.Details.Sum(d => d.Price * d.Quantity))
                    })
                    .ToList();

                var result = weeklyData.Select(wd => new
                {
                    wd.Week,
                    wd.OrdersCount,
                    Revenue = (int)wd.Revenue
                }).ToList();

                return Ok(new
                {
                    categories = result.Select(w => w.Week).ToArray(),
                    series = new[]
                    {
                new { name = "Đơn hàng", data = result.Select(w => w.OrdersCount).ToArray() },
                new { name = "Doanh thu", data = result.Select(w => w.Revenue).ToArray() }
            }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting weekly data.");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        private async Task<IActionResult> GetDailyData(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Include the end date in the query by using <= endDate.AddDays(1)
                var orders = await _context.Orders
                    .Include(o => o.Details)
                    .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate.AddDays(1))
                    .ToListAsync();

                var dailyData = orders
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new
                    {
                        Date = g.Key.ToString("yyyy-MM-dd"),
                        OrdersCount = g.Count(),
                        Revenue = g.Sum(o => o.Details.Sum(d => (decimal)d.Price * d.Quantity))
                    })
                    .ToList();

                var result = dailyData.Select(dd => new
                {
                    dd.Date,
                    dd.OrdersCount,
                    Revenue = (int)dd.Revenue
                }).ToList();

                return Ok(new
                {
                    categories = result.Select(d => d.Date).ToArray(),
                    series = new[]
                    {
                        new { name = "Đơn hàng", data = result.Select(d => d.OrdersCount).ToArray() },
                        new { name = "Doanh thu", data = result.Select(d => d.Revenue).ToArray() }
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting daily data.");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
