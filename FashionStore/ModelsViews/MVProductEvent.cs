using FashionStore.Models;

namespace FashionStore.ModelsViews
{
    public class MVProductEvent
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}
