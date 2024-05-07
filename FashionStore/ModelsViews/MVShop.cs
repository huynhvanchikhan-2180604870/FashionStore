using FashionStore.Models;

namespace FashionStore.ModelsViews
{
    public class MVShop
    {
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Product>? Products { get; set;}
        public IEnumerable<Size>? Sizes { get; set; }
        public IEnumerable<Brand>? Brands { get; set; }
    }
}
