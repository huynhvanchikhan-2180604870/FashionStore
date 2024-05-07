using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    public class ProductImage
    {
        [Key]
        public int ImageID { get; set; }
        public string UrlImage {  get; set; }
        [JsonIgnore] // Add this line
        public Product? Product { get; set; }
        public string ProductId { get; set; }
        public bool IsCover {  get; set; }
    }
}
