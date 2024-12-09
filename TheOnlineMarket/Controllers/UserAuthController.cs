using BusinessLayer;
using DtoModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace TheOnlineMarket.Controllers
{
    [ApiController]
    [Route("authUser")]
    public class UserAuthController : ControllerBase
    {
        IUserAuthRepository _Repo;
        IJson_Token _Token;
        UserManager<User> _Manager;

        public UserAuthController(IUserAuthRepository repo, IJson_Token token, UserManager<User> manager)
        {
            _Repo = repo;
            _Token = token;
            _Manager = manager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUserDto dtoUser)
        {
            if (await _Repo.UserExistAsync(dtoUser.Mail))
            {
                return BadRequest("Username is taken");
            }
            if (dtoUser == null)
            {
                return BadRequest("Invalid user data provided.");
            }


            (User User, IdentityResult Result) regResult = await _Repo.RegisterAsync(dtoUser);


            UserDto token = new UserDto
            {
                Mail = dtoUser.Mail,
                UserId = regResult.User.Id,
                Token = await _Token.CreateToken(regResult.User),
            };

            if (!regResult.Result.Succeeded)
            {
                return BadRequest(regResult.Result.Errors.Select(e => e.Description).ToList());
            }

            return Ok(token);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserCredDto userCred)
        {
            User user = await _Repo.LoginAsync(userCred);

            if (user == null || user.Email == null)
            {
                return Unauthorized("invalid mail or password");
            }

            var result = await _Manager.CheckPasswordAsync(user, userCred.Password);

            if (!result)
            {
                return Unauthorized("invalid mail or password");
            }

            UserDto token = new UserDto
            {
                Mail = user.Email,
                UserId = user.Id,
                Token = await _Token.CreateToken(user)
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
