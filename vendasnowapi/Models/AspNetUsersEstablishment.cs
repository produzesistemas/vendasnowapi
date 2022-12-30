using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("AspNetUsersEstablishment")]
    public class AspNetUsersEstablishment : BaseEntity
    {
        public int EstablishmentId { get; set; }
        public string AspNetUsersId { get; set; }

        [NotMapped]
        public ApplicationUser ApplicationUser { get; set; }
        [NotMapped]
        public virtual Establishment Establishment { get; set; }
    }
}
