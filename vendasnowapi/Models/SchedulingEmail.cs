using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("SchedulingEmail")]
    public class SchedulingEmail : BaseEntity
    {
        public bool Send { get; set; }
        public int EmailTypeId { get; set; }
        public int SchedulingId { get; set; }

        [NotMapped]
        public virtual Scheduling Scheduling { get; set; }

    }
}
