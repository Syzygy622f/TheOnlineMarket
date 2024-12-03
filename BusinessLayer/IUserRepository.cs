using DtoModels;
using Models;

namespace BusinessLayer
{
    public interface IUserRepository
    {
        Task<bool> AddCardToUserAsync(CreditCardDto creditCardDto);
        Task<bool> AddToListAsync(SaveListDto addTo);
        Task<UserInfoDto> GetUserAsync(int id);
        Task<bool> RemoveFromListAsync(int id);
        Task<bool> UpdateAsync(UserProfileDto updateUser);
    }
}