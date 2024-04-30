namespace FashionStore.Models
{
    public class Color
    {
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
