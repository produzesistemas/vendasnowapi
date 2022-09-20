using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("Product")]

    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string AspNetUsersId { get; set; }
        public decimal Value { get; set; }
        public decimal? CostValue { get; set; }
        [NotMapped]
        public List<SaleProduct> SaleProduct { get; set; }
    }
}
