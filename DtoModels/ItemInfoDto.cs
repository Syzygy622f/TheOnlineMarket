using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoModels
{
    public class ItemInfoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserPhoto { get; set; }
        public List<string> ItemPhotos { get; set; }
    }
}
