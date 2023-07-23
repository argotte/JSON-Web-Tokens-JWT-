using JWTWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //We will install the package BCrypt.NET on the nuget packages
        public static User user = new User();
        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request) 
        { 
            string passwordHash=BCrypt.Net.BCrypt.HashPassword(request.Password);
            //This hash method is Bcrypt, which is better than MD5.
            //It Generates a salt value and puts the salt value and the plain text password into its algorithm and then every time it hashes the pw, the result its diff every time.
            //This method is very safe. Do not use MD5.

            user.UserName = request.UserName;
            user.PasswordHash = passwordHash;
            return Ok(user);
        }
    }
}
