using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("SaleService")]
    public class SaleService : BaseEntity
    {
        public decimal ValueSale { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public int SaleId { get; set; }
        [NotMapped]
        public Sale Sale { get; set; }
    }
}
