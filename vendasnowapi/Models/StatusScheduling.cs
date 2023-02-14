using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("StatusScheduling")]
    public class StatusScheduling : BaseEntity
    {
       public string Description { get; set; }

        [NotMapped]
        public List<SchedulingTrack> SchedulesTrack { get; set; }
    }
}
