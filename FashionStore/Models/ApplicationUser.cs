using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName {  get; set; }
        public string Address { get; set; }
        public IEnumerable<OrderHistories> OrderHistories { get; set; }
        [JsonIgnore] // Add this line
        public ICollection<WishList>? WishLists { get; set; }
    }
}
