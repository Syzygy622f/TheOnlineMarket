using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; } 
        public string Mail { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        //public GetAge(){}

        public List<Item> Items { get; set; }
        public CreditCard Card { get; set; }
        public ResidentialArea LivingPlace { get; set; }
        public UserPhoto Photo { get; set; }
        public SaveList SaveList { get; set; }
    }
}
