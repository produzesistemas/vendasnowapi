using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("Plan")]
    public class Plan : BaseEntity
    {
        public decimal Value { get; set; }
        public bool Active { get; set; }
        public int Days { get; set; }
        public string Description { get; set; }
        public string AppName { get; set; }
    }
}
