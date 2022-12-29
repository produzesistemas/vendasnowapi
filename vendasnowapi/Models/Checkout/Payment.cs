namespace Models.Checkout
{
    public class Payment
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public int Installments { get; set; }
        public string Provider { get; set; }
        public string SoftDescriptor { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}
