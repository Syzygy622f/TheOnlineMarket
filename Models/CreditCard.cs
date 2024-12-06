using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string NameHolder { get; set; }
        public DateTime expirationDate { get; set; }
        public int cvv { get; set; }

        public int UserId { get; set; }
        public User user { get; set; }
    }
}
