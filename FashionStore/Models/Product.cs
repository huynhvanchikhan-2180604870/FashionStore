namespace FashionStore.Models
{
    public class Product
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int CategoryID {  get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int QuantityOnHand {  get; set; }
        public int MaterialID { get; set; }
        public Material Material { get; set; }
        public string? ProductDescription { get; set; }
        public IEnumerable<ProductImage> Images { get; set; }
    }
}
