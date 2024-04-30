using FashionStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace FashionStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly FashionStoreDbContext _context;
        public ProductController(FashionStoreDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
