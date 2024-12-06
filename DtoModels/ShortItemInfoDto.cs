using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoModels
{
    public class ShortItemInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string description { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
}
