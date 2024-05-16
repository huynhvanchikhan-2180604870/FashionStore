using FashionStore.Models;
using X.PagedList;

namespace FashionStore.ModelsViews
{
    public class MVShop
    {
        public IEnumerable<Category>? Categories { get; set; }
        public IPagedList<Product> Products { get; set; }
        public IEnumerable<Size>? Sizes { get; set; }
        public IEnumerable<Brand>? Brands { get; set; }
    }
}
