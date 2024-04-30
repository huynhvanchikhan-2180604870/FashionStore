using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class ProductImage
    {
        [Key]
        public int ImageID { get; set; }
        public string UrlImage {  get; set; }
        public Product? Product { get; set; }
        public string ProductId { get; set; }
        public bool IsCover {  get; set; }
    }
}
