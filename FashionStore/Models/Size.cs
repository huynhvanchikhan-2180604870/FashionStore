using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    public class Size
    {
        public int SizeID { get; set; }
        public string SizeName { get; set; }
        [JsonIgnore] // Add this line
        public ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
