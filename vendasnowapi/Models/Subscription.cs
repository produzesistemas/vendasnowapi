using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("Subscription")]
    public class Subscription : BaseEntity
    {
        public string AspNetUsersId { get; set; }
        public decimal Value { get; set; }
        public bool Active { get; set; }
        public int PlanId { get; set; }
        public DateTime SubscriptionDate { get; set; }

        [NotMapped]
        public virtual string Card_Hash { get; set; }

        [NotMapped]
        public virtual Plan Plan { get; set; }
    }
}
