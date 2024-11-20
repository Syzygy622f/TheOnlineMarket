using Models;

namespace BusinessLayer
{
    public interface IJson_Token
    {
        string CreateToken(User user);
    }
}