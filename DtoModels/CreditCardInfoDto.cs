using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoModels
{
    public class CreditCardInfoDto
    {
        public int id { get; set; }
        public string CardNumber { get; set; }
        public string NameHolder { get; set; }
        public int userId { get; set; }
    }
}
