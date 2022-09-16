using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("PaymentCondition")]

    public class PaymentCondition : BaseEntity
    {
        public string Description { get; set; }
    }
}
