namespace FashionStore.HelperClass
{
    public class Setting
    {
    }
    public class PaymentType
    {
        public static string COD = "COD";
        public static string PAYPAL = "PAYPAL";
        public static string VNPAY = "VNPAY";
    }

    public class PaymentStatus
    {
        public static short PAID = 0;
        public static short FAILED = 1;
        public static short AWAITING = 2;
    }

    public class OrderStatus
    {
        public static short CANCELLED = 0;
        public static short PROCESSING = 1;
        public static short DELIVERED = 2;
    }

	public class CurrencyConverter
	{
		public static decimal ConvertVNDToUSD(decimal amountVND, decimal exchangeRate)
		{
			decimal amountUSD = amountVND / exchangeRate;
			return Math.Round(amountUSD, 2); // Làm tròn đến 2 chữ số thập phân
		}
	}
}
