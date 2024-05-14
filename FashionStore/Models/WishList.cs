using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    public class WishList
    {
        public int WishListID { get; set; }
        public string ProductID {  get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
        public string UserID { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
