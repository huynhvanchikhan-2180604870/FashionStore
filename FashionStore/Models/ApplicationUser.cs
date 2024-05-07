using Microsoft.AspNetCore.Identity;

namespace FashionStore.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName {  get; set; }
        public string Address { get; set; }
        public IEnumerable<OrderHistories> OrderHistories { get; set; }
    }
}
