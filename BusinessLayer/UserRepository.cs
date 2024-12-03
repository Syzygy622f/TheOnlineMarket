using DatabaseLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BusinessLayer
{
    public class UserRepository : IUserRepository
    {
        private readonly Database _db;

        public UserRepository(Database db)
        {
            _db = db;
        }


        public async Task<UserInfoDto> GetUserAsync(int id)
        {
            User user = await _db.Users.Include(i => i.Card).Include(i => i.LivingPlace).Include(i => i.Photo).Include(i => i.SaveList).FirstOrDefaultAsync(x => x.Id == id);

            UserInfoDto userinfo = new UserInfoDto
            {
                id = user.Id,
                name = user.Name,
                lastName = user.LastName,
                mail = user.NormalizedEmail!,
                dateOfBirth = DateOnly.FromDateTime(user.DateOfBirth),
                livingPlace = new LivingPlaceDto
                {
                    Id = user.LivingPlace.Id,
                    City = user.LivingPlace.City,
                    Address = user.LivingPlace.Address,
                    PostCode = user.LivingPlace.PostCode,
                },
                photo = new UserPhotoDto
                {
                    Id = user.Photo.Id,
                    Url = user.Photo.Url
                }
            };

            return userinfo;
        }

        public async Task<bool> UpdateAsync(UserProfileDto updateUser)
        {
            User? existingUser = await _db.Users
                .Include(u => u.Photo)
                .Include(u => u.LivingPlace)
                .FirstOrDefaultAsync(u => u.Id == updateUser.Id);

            if (existingUser == null)
            {
                return false;
            }
            // Update simple properties
            existingUser.Name = updateUser.Name!;
            existingUser.LastName = updateUser.LastName!;
            existingUser.NormalizedEmail = updateUser.Mail;
            existingUser.DateOfBirth = updateUser.DateOfBirth!.Value.ToDateTime(TimeOnly.MinValue);

            
            if (updateUser.Photo != null)
            {
                if (existingUser.Photo == null)
                {
                    existingUser.Photo = new UserPhoto
                    {
                        Url = updateUser.Photo.Url
                    };
                }
                else if (existingUser.Photo.Url != updateUser.Photo.Url)
                {
                    existingUser.Photo.Url = updateUser.Photo.Url;
                }
            }

            // Update LivingPlace if provided
            if (updateUser.LivingPlace != null)
            {
                if (existingUser.LivingPlace == null)
                {
                    existingUser.LivingPlace = new ResidentialArea
                    {
                        City = updateUser.LivingPlace.City,
                        PostCode = updateUser.LivingPlace.PostCode,
                        Address = updateUser.LivingPlace.Address
                    };
                }
                else
                {
                    existingUser.LivingPlace.City = updateUser.LivingPlace.City;
                    existingUser.LivingPlace.PostCode = updateUser.LivingPlace.PostCode;
                    existingUser.LivingPlace.Address = updateUser.LivingPlace.Address;
                }
            }

            // Update the database
            _db.Users.Update(existingUser);
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

        public async Task<bool> AddToListAsync(SaveListDto addTo)
        {
            if (addTo == null)
                throw new ArgumentNullException(nameof(addTo));

            // Create a new SaveList entity
            var saveList = new SaveList
            {
                UserId = addTo.UserId,
                ItemId = addTo.ItemId
            };

            // Attach user if necessary to avoid duplication
            if (saveList.UserId > 0)
            {
                var user = _db.Users.Find(saveList.UserId);
                if (user != null)
                {
                    _db.Attach(user);
                }
            }

            // Add the SaveList entity to the database
            _db.SaveLists.Add(saveList);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveFromListAsync(int id)
        {
            var saveList = await _db.SaveLists.FirstOrDefaultAsync(sl => sl.Id == id);

            if (saveList == null)
                return false;

            // Remove the entity from the DbSet
            _db.SaveLists.Remove(saveList);

            await _db.SaveChangesAsync();

            return true;
        }




        public async Task<bool> AddCardToUserAsync(CreditCardDto creditCardDto)
        {
            CreditCard credit = new CreditCard
            {
                CardNumber = creditCardDto.CardNumber,
                NameHolder = creditCardDto.NameHolder,
                expirationDate = creditCardDto.expirationDate,
                cvv = creditCardDto.cvv,
                UserId = creditCardDto.UserId
            };

            await _db.CreditCards.AddAsync(credit);
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
