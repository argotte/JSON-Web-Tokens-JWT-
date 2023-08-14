using JWTWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //We will install the package BCrypt.NET on the nuget packages
        public static User user = new User();
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config=config;
        }
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

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            if(user.UserName!=request.UserName) 
            {
                return BadRequest("User not found.");
            }
            if(!BCrypt.Net.BCrypt.Verify(request.Password,user.PasswordHash)) 
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name,user.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


    }
}
