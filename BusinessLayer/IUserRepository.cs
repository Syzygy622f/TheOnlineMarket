using DtoModels;

namespace BusinessLayer
{
    public interface IUserRepository
    {
        Task<bool> AddCardToUserAsync(CreditCardDto creditCardDto);
        Task<bool> AddToListAsync(SaveListDto addTo);
        Task<bool> DeleteCard(int id);
        Task<List<CreditCardInfoDto>> GetCards(int id);
        Task<List<ShortItemInfoDto>> GetItemsFromSaveList(int id);
        Task<UserInfoDto> GetUserAsync(int id);
        Task<bool> RemoveFromListAsync(int id);
        Task<bool> UpdateAsync(UserProfileDto updateUser);
    }
}