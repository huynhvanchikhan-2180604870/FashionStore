using System.Text.Json.Serialization;

namespace FashionStore.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public string? EventName { get; set; }  
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [JsonIgnore]
        public ICollection<ProductEvent>? ProductEvents { get; set; }
        [JsonIgnore]
        public ICollection<Banner>? Banners { get; set; }
    }
}
