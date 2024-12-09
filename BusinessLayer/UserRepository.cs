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
            User user = await _db.Users.Include(i => i.Card).Include(i => i.LivingPlace).Include(i => i.SaveList).FirstOrDefaultAsync(x => x.Id == id);



            UserInfoDto userinfo = new UserInfoDto
            {
                id = user.Id,
                name = user.Name,
                lastName = user.LastName,
                mail = user.Email!,
                dateOfBirth = user.DateOfBirth,
                livingPlace = new LivingPlaceDto
                {
                    Id = user.LivingPlace.Id,
                    City = user.LivingPlace.City,
                    Address = user.LivingPlace.Address,
                    PostCode = user.LivingPlace.PostCode,
                },

            };

            return userinfo;
        }

        public async Task<bool> UpdateAsync(UserProfileDto updateUser)
        {
            // Fetch the existing user with related data
            User? existingUser = await _db.Users
                .Include(u => u.LivingPlace)
                .FirstOrDefaultAsync(u => u.Id == updateUser.Id);

            if (existingUser == null)
            {
                return false;
            }

            // Update simple properties only if provided
            if (!string.IsNullOrEmpty(updateUser.Name))
                existingUser.Name = updateUser.Name;

            if (!string.IsNullOrEmpty(updateUser.LastName))
                existingUser.LastName = updateUser.LastName;

            if (!string.IsNullOrEmpty(updateUser.Mail))
                existingUser.Email = updateUser.Mail;

            if (updateUser.DateOfBirth.HasValue)
                existingUser.DateOfBirth = updateUser.DateOfBirth.Value;


            // Update LivingPlace if provided
            if (updateUser.LivingPlace != null)
            {
                if (existingUser.LivingPlace == null)
                {
                    existingUser.LivingPlace = new ResidentialArea
                    {
                        City = updateUser.LivingPlace.City,
                        PostCode = updateUser.LivingPlace.PostCode ?? 0,
                        Address = updateUser.LivingPlace.Address
                    };
                }
                else
                {
                    if (!string.IsNullOrEmpty(updateUser.LivingPlace.City))
                        existingUser.LivingPlace.City = updateUser.LivingPlace.City;

                    if (updateUser.LivingPlace.PostCode.HasValue)
                        existingUser.LivingPlace.PostCode = updateUser.LivingPlace.PostCode.Value;

                    if (!string.IsNullOrEmpty(updateUser.LivingPlace.Address))
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


        public async Task<List<ShortItemInfoDto>> GetItemsFromSaveList(int id)//kig mere ind på det
        {
            return await _db.SaveLists.Where(x => x.UserId == id)
                .Join(_db.Items,
            saveList => saveList.ItemId,
            item => item.Id,
            (saveList, item) => new ShortItemInfoDto
            {
                Id = item.Id,
                Title = item.Title,
                Price = item.Price,
                description = item.Description,
                Photos = item.itemPhotos
                    .Select(photo => new PhotoDto
                    {
                        Id = photo.Id,
                        Url = photo.Url,
                        IsMain = photo.IsMain
                    }).ToList()
            })
        .ToListAsync();
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
        public async Task<List<CreditCardInfoDto>> GetCards(int id)
        {
            return await _db.CreditCards.Where(x => x.UserId == id)
        .Select(card => new CreditCardInfoDto
        {
            id = card.Id,
            CardNumber = card.CardNumber.Substring(card.CardNumber.Length - 4),
            NameHolder = card.NameHolder,
            userId = card.UserId
        }).ToListAsync();
        }

        public async Task<bool> DeleteCard(int id)
        {
            CreditCard? cardToDelete = await _db.CreditCards.FirstOrDefaultAsync(x => x.Id == id);
            if (cardToDelete == null)
            {
                return false;
            }

            _db.CreditCards.Remove(cardToDelete);
            await _db.SaveChangesAsync();
            return true;
        }


    }
}
