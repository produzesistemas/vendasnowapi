using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("SchedulingTrack")]
    public class SchedulingTrack : BaseEntity
    {
        public System.DateTime DateTrack { get; set; }
        public int SchedulingId { get; set; }
        public int StatusSchedulingId { get; set; }

        [NotMapped]
        public virtual StatusScheduling StatusScheduling { get; set; }
        [NotMapped]
        public virtual Scheduling Scheduling { get; set; }

    }
}
