using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoModels
{
    public class UserInfoDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string mail { get; set; }
        public UserPhotoDto photo { get; set; }
        public LivingPlaceDto livingPlace { get; set; }
    }
}
