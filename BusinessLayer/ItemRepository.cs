using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer;
using DtoModels;
using Microsoft.EntityFrameworkCore;
using Models;
using Mysqlx.Crud;

namespace BusinessLayer
{
    public class ItemRepository : IItemRepository
    {
        Database db = new();

        public async Task<List<Item>> GetAllAsync() //henter alt data i tabelen item
        {
            List<Item> items = new();

            try
            {
                items = await db.Items.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
            return items;
        }

        public async Task<ItemInfoDto> GetAsync(int id)
        {
            Item? item;
            item = await db.Items.Include(x => x.user)
                .Include(x => x.user.LivingPlace)
                .Include(x => x.user.Photo)
                .Include(x => x.photos)
                .FirstOrDefaultAsync(x => x.Id == id);



            ItemInfoDto itemInfo = new ItemInfoDto
            {
                Name = item.user.Name,
                LastName = item.user.Name,
                City = item.user.LivingPlace.City,
                UserPhoto = item.user.Photo.Url,
                Title = item.Title,
                Price = item.Price,
                Description = item.Description,
                CreatedAt = item.CreatedAt,
                ItemPhotos = item.photos.Select(x => x.Url).ToList()
            };

            return itemInfo;
        }

        public async Task<bool> Create(ItemDto itemDto)
        {
            Item newItem = new Item
            {
                Title = itemDto.Title,
                Description = itemDto.Description,
                Price = itemDto.Price,
                UserId = itemDto.UserId,
                photos = itemDto.Photos.Select(p => new ItemPhoto
                {
                    IsMain = p.IsMain,
                    Url = p.Url
                }).ToList()
            };

            await db.Items.AddAsync(newItem);
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

        public async Task<bool> Update(Item UpdateItem)
        {

            Item? existingItem = await db.Items
                .Include(x => x.photos)
                .FirstOrDefaultAsync(x => x.Id == UpdateItem.Id);

            existingItem.photos.RemoveAll(existingPhoto => !UpdateItem.photos.Any(newPhoto => newPhoto.Id == existingPhoto.Id));


            foreach (var newPhoto in UpdateItem.photos)
            {
                if (!existingItem.photos.Any(existingPhoto => existingPhoto.Id == newPhoto.Id))
                {
                    existingItem.photos.Add(newPhoto);
                }
            }

            Item Item = new Item
            {
                Id = UpdateItem.Id,
                Title = UpdateItem.Title,
                Description = UpdateItem.Description,
                CreatedAt = UpdateItem.CreatedAt, //har mulighvis ik brug for CreatedAt
                Price = UpdateItem.Price,
                SaveListId = UpdateItem.SaveListId,
                UserId = UpdateItem.UserId,
                photos = existingItem.photos
            };

            db.Items.Update(Item);
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


        public async Task<bool> Delete(int id) //fjerner en item ud fra den id
        {
            Item? item = await db.Items.FirstOrDefaultAsync(x => x.Id == id);
            db.Items.Remove(item);
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
