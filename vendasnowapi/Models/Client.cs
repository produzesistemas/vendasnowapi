using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Models
{
    [Table("Client")]
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public string Telephone { get; set; }
        public DateTime CreateDate { get; set; }
        public string ApplicationUserId { get; set; }


    }
}
