using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("SchedulingService")]
    public class SchedulingService : BaseEntity
    {
        public int SchedulingId { get; set; }
        public int ServiceId { get; set; }
        public decimal Value { get; set; }

        [NotMapped]
        public virtual Service Service { get; set; }
        [NotMapped]
        public virtual Scheduling Scheduling { get; set; }

    }
}
