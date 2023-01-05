using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Establishment")]
    public class Establishment : BaseEntity
    {
        public string ImageName { get; set; }
        public string Name { get; set; }
        public string Responsible { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public string Address { get; set; }
        public string Cnpj { get; set; }
        public bool Active { get; set; }
        public string AspNetUsersId { get; set; }
        public bool Scheduling { get; set; }

    }
}
