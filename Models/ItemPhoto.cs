using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class ItemPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }

        public int ItemId { get; set; }
        [JsonIgnore]
        public Item item { get; set; }
    }
}
