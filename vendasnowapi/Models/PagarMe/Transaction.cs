
namespace Models.PagarMe
{
    public class Transaction
    {
        public string amount { get; set; }
        public string api_key { get; set; }
        public string capture { get; set; }
        public string card_hash { get; set; }
        public Customer Customer { get; set; }

    }
}
