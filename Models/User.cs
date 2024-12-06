using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; } 

        public List<Item> Items { get; set; }
        public List<CreditCard> Card { get; set; }
        public ResidentialArea LivingPlace { get; set; }
        public UserPhoto Photo { get; set; }
        public List<SaveList> SaveList { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}