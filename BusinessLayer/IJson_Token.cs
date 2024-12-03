using DtoModels;
using Models;

namespace BusinessLayer
{
    public interface IJson_Token
    {
        Task<string> CreateToken(User user);
    }
}