using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("OpeningHours")]
    public class OpeningHours : BaseEntity
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        
        public int Weekday { get; set; }
        public bool Active { get; set; }
        public int EstablishmentId { get; set; }
        public string ApplicationUserId { get; set; }
        public string UpdateAspNetUsersId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [NotMapped]
        public Establishment Establishment { get; set; }
    }
}
