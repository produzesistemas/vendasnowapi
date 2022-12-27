using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Establishment : BaseEntity
    {

        public string Name { get; set; }
        public string Responsible { get; set; }
        public int? TypeId { get; set; }
        public string Address { get; set; }
        public string Cnpj { get; set; }
    }
}
