using BusinessLayer;
using DtoModels;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace TheOnlineMarket.Controllers
{
    [ApiController]
    [Route("Items")]
    public class ItemController : ControllerBase
    {
        IItemRepository _Repo;
        public ItemController(IItemRepository repo)
        {
            _Repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            List<Item> items = await _Repo.GetAllAsync();

            if (items == null)
            {
                return NotFound("an error come, please try again later");
            }
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            ItemInfoDto item = await _Repo.GetAsync(id);

            if (item == null)
            {
                return NotFound("an error come, please try again later");
            }
            return Ok(item);
        }

        [HttpPost]
        [Route("/item")]
        public async Task<IActionResult> CreateAsync(ItemDto item)
        {

            bool succes = false;
            if (item != null)
            {
                try
                {
                    succes = await _Repo.Create(item);
                }
                catch (Exception)
                {
                    return NotFound("an error come, please try again later");
                }
                if (succes)
                {
                    return Ok(succes);
                }
            }
            return BadRequest("didn't give the relevant data needed to create the item");
        }

        [HttpGet]
        [Route("itemUserId/{userId}")]
        public async Task<IActionResult> GetMyProducts(int userId)
        {

            List<ShortItemInfoDto> products = await _Repo.GetItemsByIdAsync(userId);

            if (products == null || !products.Any())
            {
                return NotFound(new { Message = "No products found for this user." });
            }

            return Ok(products);
        }



        [HttpPut]
        public async Task<IActionResult> Update(ItemDto item)
        {
            bool succes = false;
            if (item != null)
            {
                try
                {
                    succes = await _Repo.Update(item);
                }
                catch (Exception)
                {
                    return NotFound("an error come, please try again later");
                }
                if (succes)
                {
                    return Ok(succes);
                }
            }
            return BadRequest("didn't give the relevant data needed to update the item");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWare(int id)
        {
            if (id != 0)
            {
                return Ok(await _Repo.Delete(id));
            }
            return BadRequest("didn't give the relevant data needed to Delete the item");
        }

    }
}
