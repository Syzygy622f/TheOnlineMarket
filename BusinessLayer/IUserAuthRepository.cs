using DtoModels;
using Models;

namespace BusinessLayer
{
    public interface IUserAuthRepository
    {
        Task<byte[]> CorrectPasswordAsync(string password, User user);
        Task<bool> Delete(int id);
        Task<User> LoginAsync(UserCredDto LoginDto);
        Task<User> RegisterAsync(CreateUserDto RegUser);
        Task<bool> UserExistAsync(string mail);
    }
}