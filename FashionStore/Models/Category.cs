using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        [JsonIgnore] // Add this line if needed
        public ICollection<Product>? Products { get; set; }
    }
}
