using DatabaseLayer;
using DtoModels;
using Microsoft.AspNetCore.Identity;
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
        private readonly Database _db;
        HashPassword hashing = new();
        UserManager<User> _userManager;
        public UserAuthRepository(UserManager<User> userManager, Database db)
        {
            _db = db;
            _userManager = userManager;
        }


        public async Task<(User User, IdentityResult Result)> RegisterAsync(CreateUserDto RegUser)
        {
            User user = new User();
            user.Name = RegUser.Name;
            user.LastName = RegUser.LastName;
            user.DateOfBirth = RegUser.DateOfBirth;
            user.Email = RegUser.Mail;
            user.UserName = RegUser.Name + "1234";
            user.LivingPlace = new ResidentialArea
            {
                City = RegUser.City,
                Address = RegUser.Address,
                PostCode = RegUser.PostCode,
            };
            user.Photo = new UserPhoto
            {
                Url = RegUser.Url
            };

            IdentityResult result = await _userManager.CreateAsync(user, RegUser.Password);

            return (user, result);
        }

        public async Task<User> LoginAsync(UserCredDto logindto)
        {
            User? user = await _db.Users.FirstOrDefaultAsync(x => x.Email == logindto.Mail);
            return user!;
        }

        public async Task<bool> UserExistAsync(string mail)
        {
            return await _db.Users.AnyAsync(x => x.Email == mail);
        }


        public async Task<bool> Delete(int id) //fjerner en item ud fra den id
        {
            User? user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            _db.Users.Remove(user);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
