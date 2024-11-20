using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User user { get; set; }
        public int SaveListId { get; set; }
        public SaveList saveList { get; set; }
        public List<ItemPhoto> photos { get; set; }
    }
}
