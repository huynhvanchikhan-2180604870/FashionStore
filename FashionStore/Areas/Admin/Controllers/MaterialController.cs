using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class MaterialController : Controller
    {
        private readonly FashionStoreDbContext _context;
        public MaterialController(FashionStoreDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var materials = await _context.Materials.ToListAsync();   
            return View(materials);
        }

        public  IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Material material)
        {
            if(ModelState.IsValid)
            {
                _context.Materials.Add(material);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("index");
        }
    }
}
