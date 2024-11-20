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
        public int CardNumber { get; set; }

        public int UserId { get; set; }
        public User user { get; set; }
    }
}
