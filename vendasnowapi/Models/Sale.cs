using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("Sale")]
    public class Sale : BaseEntity
    {
        public string AspNetUsersId { get; set; }
        public string Obs { get; set; }
        public int PaymentConditionId { get; set; }
        public int ClientId { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime CreateDate { get; set; }

        [NotMapped]
        public virtual List<SaleProduct> SaleProducts { get; set; }
        [NotMapped]
        public virtual List<SaleService> SaleServices { get; set; }
        [NotMapped]
        public virtual List<Account> Accounts { get; set; }
        [NotMapped]
        public virtual PaymentCondition PaymentCondition { get; set; }
        [NotMapped]
        public virtual Client Client { get; set; }
    }
}
