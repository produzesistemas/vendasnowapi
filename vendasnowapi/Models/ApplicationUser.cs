using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string ImageName { get; set; }
        public string EstablishmentName { get; set; }
        public string ResponsibleName { get; set; }
        public int? TypeId { get; set; }
        public string Address { get; set; }
        public string Cnpj { get; set; }

        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public string Role { get; set; }

        public static explicit operator ApplicationUser(string v)
        {
            throw new NotImplementedException();
        }


        public ApplicationUser()
        {
            
        }

    }
}