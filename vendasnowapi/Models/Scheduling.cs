using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Scheduling")]
    public class Scheduling : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public string UpdateAspNetUsersId { get; set; }
        public int EstablishmentId { get; set; }
        public DateTime SchedulingDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [NotMapped]
        public virtual List<SchedulingService> SchedulingService { get; set; }
        [NotMapped]
        public virtual List<SchedulingTrack> SchedulingTrack { get; set; }
        [NotMapped]
        public virtual List<SchedulingReview> SchedulingReview { get; set; }
        [NotMapped]
        public virtual List<SchedulingEmail> SchedulingEmail { get; set; }

        [NotMapped]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [NotMapped]
        public virtual ApplicationUserDTO ApplicationUserDTO { get; set; }
        [NotMapped]
        public virtual Establishment Establishment { get; set; }

        [NotMapped]
        public virtual string UserName { get; set; }
        [NotMapped]
        public virtual string Email { get; set; }

        public Scheduling()
        {
            SchedulingTrack = new List<SchedulingTrack>();
            SchedulingEmail = new List<SchedulingEmail>();
        }
    }
}
