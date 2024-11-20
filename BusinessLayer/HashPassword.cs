using DtoModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class HashPassword
    {

        public async Task<User> HashPasswordAsync(UserCredDto userDto)
        {
            using var hmac = new HMACSHA512();

            var user = new User
            {
                Mail = userDto.Mail,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password)),
                PasswordSalt = hmac.Key
            };

            return user;
        }


    }
}
