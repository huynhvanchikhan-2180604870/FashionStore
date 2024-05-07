using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models
{
    public class OrderHistories
    {
        [Key]
        public int HistoryID { get; set; }
        public int? OrderID { get; set; }
        public string? UserID { get; set; }
        public OrderDetail? OrderDetail { get; set; }
        public Order? Order { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
