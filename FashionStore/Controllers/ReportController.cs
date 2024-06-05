using FashionStore.Services;
using DinkToPdf;
using DinkToPdf.Contracts;
using FashionStore.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using FashionStore.Data;
using Microsoft.EntityFrameworkCore;
using FashionStore.Models;

namespace FashionStore.Controllers
{
    public class ReportController : Controller 
    {
        private readonly IPDFService _pDFService;
        private readonly FashionStoreDbContext _context;
        public ReportController(IPDFService pDFService, FashionStoreDbContext context)
        {
            _pDFService = pDFService;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ExportOrderPdf(int id)
        {
            var order = await _context.Orders.Include(o => o.User).Include(o => o.Details)
                .ThenInclude(de => de.Product).ThenInclude(p => p.ProductDetails).ThenInclude(x => x.Size).FirstOrDefaultAsync(x => x.OrderID == id);
            string html = await this.RenderViewAsync<Order>(RouteData, "OrderDetail", order);
            var result = _pDFService.GenaratePDF(html);
            return File(result, "application/pdf", $"{DateTime.Now.Ticks}.pdf");
        }
    }
}
