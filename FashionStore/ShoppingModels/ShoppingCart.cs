namespace FashionStore.ShoppingModels
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal? Discount { get; set; }
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoveItem(string producId)
        {
            Items.RemoveAll(i => i.ProductId == producId);
        }
    }
}
