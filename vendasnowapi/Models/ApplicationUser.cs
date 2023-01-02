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


        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public string Role { get; set; }
        [NotMapped]
        public Establishment Establishment { get; set; }

        public static explicit operator ApplicationUser(string v)
        {
            throw new NotImplementedException();
        }


        public ApplicationUser()
        {
            
        }

    }
}