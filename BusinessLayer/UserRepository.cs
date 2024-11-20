using DatabaseLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserRepository : IUserRepository
    {
        Database db = new();


        public async Task<User> GetUserAsync(int id)
        {
            User user;
            return user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(User Updateuser)
        {
            User? existingUser = await db.Users
                .Include(u => u.Photo)
                .Include(u => u.LivingPlace)
                .FirstOrDefaultAsync(u => u.Id == Updateuser.Id);

            if (existingUser == null)
            {
                return false;
            }


            existingUser.Name = Updateuser.Name;
            existingUser.LastName = Updateuser.LastName;
            existingUser.DateOfBirth = Updateuser.DateOfBirth;
            existingUser.Mail = Updateuser.Mail;


            if (Updateuser.Photo != null)
            {
                if (existingUser.Photo == null || !existingUser.Photo.Equals(Updateuser.Photo))
                {
                    existingUser.Photo = Updateuser.Photo;
                }
            }

            if (Updateuser.LivingPlace != null)
            {
                if (existingUser.LivingPlace == null || !existingUser.LivingPlace.Equals(Updateuser.LivingPlace))
                {
                    existingUser.LivingPlace = Updateuser.LivingPlace;
                }
            }

            db.Users.Update(existingUser);
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

        public async Task<bool> AddToListAsync(SaveList addTo)
        {
            if (addTo == null)
                throw new ArgumentNullException(nameof(addTo));


            // Attach the user if it exists, to avoid duplicating the user entity
            if (addTo.UserId > 0)
                db.Attach(addTo.user);

            // Add the SaveList entity to the database
            db.SaveLists.Add(addTo);

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveFromListAsync(int id)
        {
            var saveList = await db.SaveLists.FirstOrDefaultAsync(sl => sl.Id == id);

            if (saveList == null)
                return false;

            // Remove the entity from the DbSet
            db.SaveLists.Remove(saveList);

            await db.SaveChangesAsync();

            return true;
        }


        public async Task<bool> AddCardToUserAsync(CreditCard card)
        {
            CreditCard credit = new CreditCard
            {
                CardNumber = card.CardNumber,
                UserId = card.Id
            };

            await db.CreditCards.AddAsync(credit);
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
