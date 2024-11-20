using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoModels
{
    public class ItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int UserId { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }

    public class PhotoDto
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}
