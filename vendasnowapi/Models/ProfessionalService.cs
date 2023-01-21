using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("ProfessionalService")]
    public class ProfessionalService : BaseEntity
    {
        public int ServiceId { get; set; }
        public int ProfessionalId { get; set; }

        [NotMapped]
        public Service Service { get; set; }
        [NotMapped]
        public virtual Professional Professional { get; set; }
    }
}
