using BusinessLayer;
using DtoModels;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace TheOnlineMarket.Controllers
{
    public class UserAuthController : ControllerBase
    {
        IUserAuthRepository _Repo;
        IJson_Token _Token;

        UserAuthController(IUserAuthRepository repo, IJson_Token token)
        {
            _Repo = repo;
            _Token = token;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUserDto dtoUser)
        {
            if (await _Repo.UserExistAsync(dtoUser.Mail))
            {
                return BadRequest("Username is taken");
            }
            User user = await _Repo.RegisterAsync(dtoUser);

            UserDto token = new UserDto
            {
                Mail = dtoUser.Mail,
                Token = _Token.CreateToken(user)
            };

            return Ok(token);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserCredDto userCred)
        {
            User user = await _Repo.LoginAsync(userCred);

            if (user == null)
            {
                return Unauthorized("invalid mail");
            }

            byte[] CorrectPassword = await _Repo.CorrectPasswordAsync(userCred.Password, user);

            for (int i = 0; i < CorrectPassword.Length; i++)
            {
                if (CorrectPassword[i] != user.PasswordHash[i])
                {
                    return Unauthorized("invalid password");
                }
            }

            UserDto token = new UserDto
            {
                Mail = user.Mail,
                Token = _Token.CreateToken(user)
            };
            return Ok(token);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    if (id != 0)
        //    {
        //        return Ok(await _Repo.Delete(id));
        //    }
        //    return BadRequest("didn't give the relevant data needed to remove the item");
        //}
    }
}
