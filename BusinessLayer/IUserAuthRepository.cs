using DtoModels;
using Microsoft.AspNetCore.Identity;
using Models;

namespace BusinessLayer
{
    public interface IUserAuthRepository
    {
        Task<bool> Delete(int id);
        Task<User> LoginAsync(UserCredDto logindto);
        Task<(User User, IdentityResult Result)> RegisterAsync(CreateUserDto RegUser);
        Task<bool> UserExistAsync(string mail);
    }
}