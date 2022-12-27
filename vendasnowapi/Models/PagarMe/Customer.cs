namespace Models.PagarMe
{
    public class Customer
    {
        public string document_number { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public Phone Phone { get; set; }
        public Address Address { get; set; }
    }
}
