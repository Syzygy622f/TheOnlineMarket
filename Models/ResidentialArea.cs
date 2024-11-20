using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ResidentialArea
    {
        public int Id { get; set; }
        public string City { get; set; }
        [MaxLength(4)]
        public int PostCode { get; set; }
        public string Address { get; set; }

        public int UserId { get; set; }
        public User user { get; set; }
    }
}
