using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Database _database;

        public UsersController(Database database)
        {
            _database = database;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                {
                    return BadRequest("Username and password are required");
                }

                var userDB = await _database.User.FindAsync(user.Username);
                if (userDB != null)
                {
                    return Conflict("User already exists");
                }

                _database.User.Add(user);

                await _database.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return StatusCode(500);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                {
                    return BadRequest("Username and password are required");
                }

                var userDB = await _database.User.FindAsync(user.Username);
                if (userDB == null)
                {
                    return NotFound("User not found");
                }

                if (userDB.Password != user.Password)
                {
                    return Unauthorized("Invalid password");
                }

                HttpContext.Session.SetString("username", user.Username);
                return Ok();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }
    }
}