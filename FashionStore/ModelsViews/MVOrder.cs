using FashionStore.Models;
using FashionStore.ShoppingModels;

namespace FashionStore.ModelsViews
{
    public class MVOrder
    {
        public Order Order { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
