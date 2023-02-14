using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Establishment")]
    public class Establishment : BaseEntity
    {
        public string ImageName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public string Address { get; set; }
        public string Alias { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Cnpj { get; set; }
        public bool Active { get; set; }

        [NotMapped]
        public virtual List<Subscription> Subscriptions { get; set; }
        [NotMapped]
        public virtual List<Service> Services { get; set; }

        [NotMapped]
        public virtual List<OpeningHours> OpeningHours { get; set; }
        [NotMapped]
        public virtual List<Scheduling> Schedulings { get; set; }

    }
}
