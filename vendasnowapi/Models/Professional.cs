using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("Professional")]

    public class Professional : BaseEntity
    {
        public string ImageName { get; set; }
        public string Name { get; set; }
        public string ApplicationUserId { get; set; }
        public bool Active { get; set; }
        public int EstablishmentId { get; set; }
        public string UpdateAspNetUsersId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [NotMapped]
        public string Base64 { get; set; }
        [NotMapped]
        public virtual List<ProfessionalService> ProfessionalService { get; set; }
    }
}
