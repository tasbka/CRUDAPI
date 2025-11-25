using BussinessLogic.Users;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi;

[ApiController]
[Route("Users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AddUserRequest request)
        {
            try
            {
                await _userService.CreateAsync(request.Username, request.Email, request.Password);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
}
