using FashionStore.Models;

namespace FashionStore.ShoppingModels
{
    public class CartItem
    {
        public string? ProductId { get; set; }
        public Product? Product { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? SizeID { get; set; }
        public Size? Size { get; set; }
        public IEnumerable<ProductDetail> ProductDetails { get; set; }
        public IEnumerable<ProductImage> Images { get; set; }
    }
}
