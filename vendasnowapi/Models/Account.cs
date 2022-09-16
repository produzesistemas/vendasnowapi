using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("Account")]
    public class Account : BaseEntity
    {
        public decimal Value { get; set; }
        public decimal? AmountPaid { get; set; }
        public int SaleId { get; set; }
        public int Status { get; set; }
        public int MRequestCode { get; set; }
        public int UniqueIDNotification { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? DateOfPayment { get; set; }

        [NotMapped]
        public Sale Sale { get; set; }

    }
}
