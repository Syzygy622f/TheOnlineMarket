using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoModels
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Url { get; set; }
        public string City { get; set; }
        public int PostCode { get; set; }
        public string Address { get; set; }
    }
}
