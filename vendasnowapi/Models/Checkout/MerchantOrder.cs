namespace Models.Checkout
{
    public class MerchantOrder
    {
        public string MerchantOrderId { get; set; }
        public Payment Payment { get; set; }

    }
}
