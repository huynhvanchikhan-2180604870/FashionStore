namespace FashionStore.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string UserID { get; set; }
        public string? AddressShipping {  get; set; }
        public DateTime OrderDate { get; set; }
        public bool Status { get; set; }
        public bool IsPayment {  get; set; }
        public string PaymentType {  get; set; }
        public string? Notes { get; set; }
        public ApplicationUser User { get; set; }
        public decimal Disscount { get; set; }
    }
}
