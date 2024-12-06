using DtoModels;
using Models;

namespace BusinessLayer
{
    public interface IItemRepository
    {
        Task<bool> Create(ItemDto itemDto);
        Task<bool> Delete(int id);
        Task<List<Item>> GetAllAsync();
        Task<ItemInfoDto> GetAsync(int id);
        Task<List<ShortItemInfoDto>> GetItemsByIdAsync(int id);
        Task<bool> Update(ItemDto UpdateItem);
    }
}