﻿using FashionStore.Data;
using FashionStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly FashionStoreDbContext _dbContext;
        public CategoryController(FashionStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            // Lấy tất cả các danh mục từ cơ sở dữ liệu
            var categories = await _dbContext.Categories.ToListAsync();

            // Lặp qua từng danh mục để lấy các sản phẩm tương ứng
            foreach (var category in categories)
            {
                // Lấy các sản phẩm thuộc về danh mục hiện tại
                var productsInCategory = await _dbContext.Products
                    .Where(p => p.CategoryId == category.CategoryID)
                    .ToListAsync();

                // Gán các sản phẩm cho danh mục
                category.Products = productsInCategory;
            }

            // Trả về danh sách các danh mục và sản phẩm cho view
            return View(categories);
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int name)
        {
            var category = await _dbContext.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryID == name);
            if (category!= null)
            {
                if(category.Products.Count() > 0)
                {
                    var products = await _dbContext.Products.Where(x => x.CategoryId == category.CategoryID).ToListAsync();
                    foreach (var product in products)
                    {
                        _dbContext.Products.Remove(product);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _dbContext.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryID == id);
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
    }

}
