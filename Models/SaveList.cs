using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SaveList
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User user { get; set; }
        public List<Item> items { get; set; }
    }
}
