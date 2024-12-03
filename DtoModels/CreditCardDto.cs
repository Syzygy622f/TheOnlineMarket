using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoModels
{
    public class CreditCardDto
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string NameHolder { get; set; }
        public DateOnly expirationDate { get; set; }
        public int cvv { get; set; }
        public int UserId { get; set; }
    }
}
