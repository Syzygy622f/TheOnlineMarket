using DatabaseLayer;
using DtoModels;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserAuthRepository : IUserAuthRepository
    {
        Database db = new();
        HashPassword hashing = new();

        public async Task<User> RegisterAsync(CreateUserDto RegUser)
        {
            UserCredDto userCred = new UserCredDto
            {
                Mail = RegUser.Mail,
                Password = RegUser.Password
            };

            User user = await hashing.HashPasswordAsync(userCred);

            user.LastName = RegUser.LastName;
            user.LivingPlace = new ResidentialArea
            {
                City = RegUser.City,
                Address = RegUser.Address,
                PostCode = RegUser.PostCode,
            };
            user.Photo = new UserPhoto
            {
                Url = RegUser.PhotoUrl
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            return user;
        }

        public async Task<User> LoginAsync(UserCredDto LoginDto)
        {
            User? user = await db.Users.FirstOrDefaultAsync(x => x.Mail == LoginDto.Mail);
            return user;
        }

        public async Task<byte[]> CorrectPasswordAsync(string password, User user)
        {
            using var HMAC = new HMACSHA512(user.PasswordSalt);

            byte[] computeHash = HMAC.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computeHash;
        }

        public async Task<bool> UserExistAsync(string mail)
        {
            return await db.Users.AnyAsync(x => x.Mail == mail);
        }


        public async Task<bool> Delete(int id) //fjerner en item ud fra den id
        {
            User? user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            db.Users.Remove(user);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
