using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("SchedulingReview")]
    public class SchedulingReview : BaseEntity
    {
        public System.DateTime DateReview { get; set; }
        public int SchedulingId { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; }
        public string ApplicationUserId { get; set; }

        [NotMapped]
        public virtual Scheduling Scheduling { get; set; }
        [NotMapped]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}

