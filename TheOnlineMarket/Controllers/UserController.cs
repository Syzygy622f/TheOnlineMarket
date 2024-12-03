using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using DtoModels;

namespace TheOnlineMarket.Controllers
{
    [Authorize]
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        IUserRepository _Repo;

        public UserController(IUserRepository repo)
        {
            _Repo = repo;
        }

        [HttpGet("id")]
        public async Task<IActionResult> getAsync(int id)
        {
            UserInfoDto user = await _Repo.GetUserAsync(id);

            if (user == null)
            {
                return NotFound("an error come, please try again later");
            }

            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UserProfileDto user)
        {
            bool succes = false;
            if (user != null)
            {
                try
                {
                    succes = await _Repo.UpdateAsync(user);
                }
                catch (Exception)
                {
                    return NotFound("an error come, please try again later");
                }
                if (succes)
                {
                    return Ok();
                }
            }
            return BadRequest("didn't give the relevant data needed to Update the user");
        }

        [HttpPost]
        [Route("/SaveList")]
        public async Task<IActionResult> CreateAsync(SaveListDto list)
        {
            bool succes = false;
            if (list != null)
            {
                try
                {
                    succes = await _Repo.AddToListAsync(list);
                }
                catch (Exception)
                {
                    return NotFound("an error come, please try again later");
                }
                if (succes)
                {
                    return Ok();
                }
            }
            return BadRequest("didn't give the relevant data needed to create the item");
        }
        [HttpPost]
        [Route("/Card")]
        public async Task<IActionResult> CreateAsync(CreditCardDto card)
        {
            bool succes = false;
            if (card != null)
            {
                try
                {
                    succes = await _Repo.AddCardToUserAsync(card);
                }
                catch (Exception)
                {
                    return NotFound("an error come, please try again later");
                }
                if (succes)
                {
                    return Ok();
                }
            }
            return BadRequest("didn't give the relevant data needed to create the item");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFromlist(int id)
        {
            if (id != 0)
            {
                return Ok(await _Repo.RemoveFromListAsync(id));
            }
            return BadRequest("didn't give the relevant data needed to remove the item");
        }
    }
}
