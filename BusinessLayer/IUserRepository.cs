using Models;

namespace BusinessLayer
{
    public interface IUserRepository
    {
        Task<bool> AddCardToUserAsync(CreditCard card);
        Task<bool> AddToListAsync(SaveList addTo);
        Task<User> GetUserAsync(int id);
        Task<bool> RemoveFromListAsync(int id);
        Task<bool> UpdateAsync(User Updateuser);
    }
}