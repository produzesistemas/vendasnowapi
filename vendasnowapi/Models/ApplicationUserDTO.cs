
namespace Models
{
    public class ApplicationUserDTO
    {
        public string Role { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string NomeImagem { get; set; }
        public string Token { get; set; }
        public virtual Subscription Subscription { get; set; }

    }
}
