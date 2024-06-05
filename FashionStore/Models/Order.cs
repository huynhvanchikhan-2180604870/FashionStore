using FashionStore.ShoppingModels;

namespace FashionStore.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string UserID { get; set; }
        public string? AddressShipping {  get; set; }
        public DateTime OrderDate { get; set; }
        public short Status { get; set; }
        public short IsPayment {  get; set; }
        public string PaymentType {  get; set; }
        public string? Notes { get; set; }
        public ApplicationUser User { get; set; }
        public decimal Disscount { get; set; }
        public IEnumerable<OrderDetail> Details { get; set; }

    }
}
