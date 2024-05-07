using FashionStore.Models;
using FashionStore.ShoppingModels;

namespace FashionStore.ModelsViews
{
    public class MVProductDetails
    {
        public IEnumerable<ProductDetail> ProductDetails { get; set; }
        public IEnumerable<ProductImage>Images { get; set; }
        public Product Product { get; set; }
        public CartItem? CartItem { get; set; }
    }
}
