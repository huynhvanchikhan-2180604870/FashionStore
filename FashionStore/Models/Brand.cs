﻿namespace FashionStore.Models
{
    public class Brand
    {
        public int BrandId { get; set; } 
        public string BrandName { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
