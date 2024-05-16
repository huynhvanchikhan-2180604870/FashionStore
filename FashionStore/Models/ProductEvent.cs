using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class ProductEvent
    {
        [Key, Column(Order = 0)]
        public string ProductID { get; set; }

        [Key, Column(Order = 1)]
        public int EventID { get; set; }

        public Product Product { get; set; }
        public Event Event { get; set; }
    }
}
