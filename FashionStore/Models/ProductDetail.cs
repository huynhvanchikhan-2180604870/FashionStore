namespace FashionStore.Models
{
    public class ProductDetail
    {
        public int ProductDetailID { get; set; }
        public string ProductID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
    }
}
