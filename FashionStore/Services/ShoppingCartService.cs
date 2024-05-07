using FashionStore.HelperClass;
using FashionStore.ShoppingModels;

namespace FashionStore.Services
{
    public class ShoppingCartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ShoppingCart GetShoppingCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cart = session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return cart;
        }
    }
}
