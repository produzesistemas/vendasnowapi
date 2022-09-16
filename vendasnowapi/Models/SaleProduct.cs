using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("SaleProduct")]
    public class SaleProduct : BaseEntity
    {
        public decimal ValueSale { get; set; }
        public decimal Quantity { get; set; }
        public int ProductId { get; set; }
        public int SaleId { get; set; }


        [NotMapped]
        public Product Product { get; set; }
        [NotMapped]
        public Sale Sale { get; set; }
    }
}
