using BussinessLogic.Users;
using BussinessLogic.Users.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
//[Route("Users")]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
         [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddUserRequest request)
        {
            Console.WriteLine($"=== REGISTER REQUEST RECEIVED ===");
            Console.WriteLine($"Username: {request?.Username}");
            Console.WriteLine($"Email: {request?.Email}");
            Console.WriteLine($"Password length: {request?.Password?.Length}");
            try
            {
                if (request == null)
                {
                    Console.WriteLine("ERROR: Request is null!");
                    return BadRequest(new 
                    {
                        success = false,
                        message = "Request body is null"
                    });
                }
                
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    Console.WriteLine("ERROR: Username is empty");
                    return BadRequest("Имя пользователя обязательно");
                }
                    
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    Console.WriteLine("ERROR: Email is empty");
                    return BadRequest("Email обязателен");
                }
                    
                if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
                {
                    Console.WriteLine($"ERROR: Password invalid (length: {request.Password?.Length})");
                    return BadRequest("Пароль должен содержать минимум 6 символов");
                }
                
                Console.WriteLine("Calling UserService.CreateAsync...");
                var user = await _userService.CreateAsync(request.Username, request.Email, request.Password);
                
                 
                Console.WriteLine($"User created successfully: {user.Id}");
                
                return Ok(new 
                {
                    success = true,
                    message = "Регистрация успешна",
                    data = user
                });
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"ArgumentException: {ex.Message}");
                return BadRequest(new 
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled Exception: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new 
                {
                    success = false,
                    message = "Произошла ошибка при регистрации"
                });
            }
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Username))
                    return BadRequest(new 
                    {
                        success = false,
                        message = "Введите имя пользователя"
                    });
            
                if (string.IsNullOrWhiteSpace(request.Password))
                    return BadRequest(new 
                    {
                        success = false,
                        message = "Введите пароль"
                    });
                
                var user = await _userService.AuthenticateAsync(request.Username, request.Password);
        
                if (user == null)
                    return Unauthorized(new 
                    {
                        success = false,
                        message = "Неверный логин или пароль"
                    });

                return Ok(new
                {
                    success = true,
                    message = "Вход выполнен успешно",
                    data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                {
                    success = false,
                    message = "Произошла ошибка при входе"
                });
            }
        }
        /*

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
               */
       [HttpGet("{id:guid}")]
       public async Task<IActionResult> GetByIdAsync(Guid id)
       {
           try
           {
               var user = await _userService.GetByIdAsync(id);
               return Ok(new 
               {
                   success = true,
                   data = user
               });
           }
           catch (ArgumentException ex)
           {
               return NotFound(new 
               {
                   success = false,
                   message = ex.Message
               });
           }
       }
}
