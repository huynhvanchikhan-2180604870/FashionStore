namespace FashionStore.Models
{
    public class Size
    {
        public int SizeID { get; set; }
        public string SizeName { get; set; }
        public IEnumerable<ProductDetail> ProductDetails { get; set; }
    }
}
