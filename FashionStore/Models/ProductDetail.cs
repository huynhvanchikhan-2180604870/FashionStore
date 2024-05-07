using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    public class ProductDetail
    {
        public int ProductDetailID { get; set; }
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public int? SizeID { get; set; }
        [JsonIgnore] // Add this line
        public Product? Product { get; set; }
        [JsonIgnore] // Add this line
        public Size? Size { get; set; }
    }
}
