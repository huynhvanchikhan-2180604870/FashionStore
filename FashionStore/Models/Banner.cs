namespace FashionStore.Models
{
    public class Banner
    {
        public int Id { get; set; }
        public string? UrlImage { get; set; }
        public int? EventID { get; set; }
        public Event? Event { get; set; }
    }
}
