using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoModels
{
    public class UserProfileDto
    {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? LastName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string? Mail { get; set; }
            public string? PasswordHash { get; set; }
            public string? PasswordSalt { get; set; }
            public LivingPlaceDto? LivingPlace { get; set; }
            public UserPhotoDto? Photo { get; set; }
    }
}
