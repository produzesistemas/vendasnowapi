using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("AspNetUsersEstablishment")]
    public class AspNetUsersEstablishment : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public int EstablishmentId { get; set; }

        [NotMapped]
        public ApplicationUser ApplicationUser { get; set; }
        [NotMapped]
        public virtual Establishment Establishment { get; set; }
    }
}
