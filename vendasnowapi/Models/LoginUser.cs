
namespace Models
{
    public class LoginUser
    {
        public string Email { get; set; }
        public string Secret { get; set; }
        public string UserName { get; set; }
        public string AppName { get; set; }
        public string ApplicationUserId { get; set; }
        public string Code { get; set; }
        public string ImageBase64 { get; set; }
        public string EstablishmentName { get; set; }
        public string ResponsibleName { get; set; }
        public int? TypeId { get; set; }
        public string Address { get; set; }
        public string Cnpj { get; set; }
        public string PhoneNumber { get; set; }
    }
}
