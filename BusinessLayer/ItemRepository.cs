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
        private readonly Database _db;

        public ItemRepository(Database db)
        {
            _db = db;
        }

        public async Task<List<Item>> GetAllAsync() //henter alt data i tabelen item
        {
            List<Item> items = new();

            try
            {
                items = await _db.Items.Include(x => x.itemPhotos).ToListAsync();
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
            item = await _db.Items.Include(x => x.user)
                .Include(x => x.user.LivingPlace)
                .Include(x => x.user.Photo)
                .Include(x => x.itemPhotos)
                .FirstOrDefaultAsync(x => x.Id == id);



            ItemInfoDto itemInfo = new ItemInfoDto
            {
                Id = item.Id,
                Name = item.user.Name,
                LastName = item.user.Name,
                City = item.user.LivingPlace.City,
                UserPhoto = item.user.Photo.Url,
                Title = item.Title,
                Price = item.Price,
                Description = item.Description,
                CreatedAt = item.CreatedAt,
                ItemPhotos = item.itemPhotos.Select(x => x.Url).ToList()
            };

            return itemInfo;
        }

        public async Task<List<ShortItemInfoDto>> GetItemsByIdAsync(int id)
        {

            List<ShortItemInfoDto> items = await _db.Items
                    .Where(p => p.UserId == id)
                    .Select(item => new ShortItemInfoDto
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Price = item.Price,
                        description = item.Description,
                        Photos = item.itemPhotos.Select(photo => new PhotoDto
                        {
                            Id = photo.Id,
                            Url = photo.Url,
                            IsMain = photo.IsMain
                        }).ToList()
                    }).ToListAsync();
            return items;
        }


        public async Task<bool> Create(ItemDto itemDto)
        {
            Item newItem = new Item
            {
                Title = itemDto.Title,
                Description = itemDto.Description,
                Price = itemDto.Price,
                UserId = itemDto.UserId,
                itemPhotos = itemDto.Photos.Select(p =>
                {
                    // Process the URL
                    string processedUrl = p.Url;
                    int jpgIndex = processedUrl.IndexOf(".jpg", StringComparison.OrdinalIgnoreCase);
                    if (jpgIndex >= 0)
                    {
                        processedUrl = processedUrl.Substring(0, jpgIndex + 4); // Retain up to ".jpg"
                    }
                    processedUrl += "?width=1600"; // Add width parameter

                    return new ItemPhoto
                    {
                        IsMain = p.IsMain,
                        Url = processedUrl
                    };
                }).ToList()
            };

            await _db.Items.AddAsync(newItem);
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

        public async Task<bool> Update(ItemDto UpdateItem)
        {

            Item? existingItem = await _db.Items
                .Include(x => x.itemPhotos)
                .FirstOrDefaultAsync(x => x.Id == UpdateItem.Id);

            if (existingItem == null)
            {
                return false;
            }

            // Update scalar properties
            existingItem.Title = UpdateItem.Title;
            existingItem.Description = UpdateItem.Description;
            existingItem.Price = UpdateItem.Price;

            // Handle photos
            existingItem.itemPhotos.RemoveAll(existingPhoto =>//der er fejl her skal rettes
                !UpdateItem.Photos.Any(newPhoto => newPhoto.Id == existingPhoto.Id));

            foreach (var newPhoto in UpdateItem.Photos)
            {
                if (!existingItem.itemPhotos.Any(existingPhoto => existingPhoto.Id == newPhoto.Id))
                {

                    existingItem.itemPhotos.Add(new ItemPhoto
                    {
                        Id = newPhoto.Id,
                        Url = newPhoto.Url,
                        IsMain = newPhoto.IsMain,
                        ItemId = existingItem.Id
                    });
                }
            }

            Item? item = await _db.Items.FindAsync(existingItem.Id);
            if (item == null)
            {
                return false;
            }

            item.Title = UpdateItem.Title;
            item.Description = UpdateItem.Description;
            item.Price = UpdateItem.Price;
            item.itemPhotos = existingItem.itemPhotos;
            

            _db.Items.Update(item);
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


        public async Task<bool> Delete(int id) //fjerner en item ud fra den id
        {
            Item? item = await _db.Items.FirstOrDefaultAsync(x => x.Id == id);
            _db.Items.Remove(item);
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
