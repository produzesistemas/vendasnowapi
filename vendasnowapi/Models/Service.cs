using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("Service")]

    public class Service : BaseEntity
    {
        public string ImageName { get; set; }
        public string Description { get; set; }
        public string ApplicationUserId { get; set; }
        public decimal Value { get; set; }
        public int? Minute { get; set; }
        public bool Active { get; set; }
        public int EstablishmentId { get; set; }
        public string UpdateAspNetUsersId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [NotMapped]
        public string Base64 { get; set; }
    }
}
