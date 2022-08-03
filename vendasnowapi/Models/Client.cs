using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
[Table("Client")]
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public string AspNetUsersId { get; set; }


   }
}
