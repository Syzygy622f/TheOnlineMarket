using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public int UserId { get; set; }
        public User user { get; set; }
    }
}
