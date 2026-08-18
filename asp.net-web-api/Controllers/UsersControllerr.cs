using asp.net_web_api.Data;
using asp.net_web_api.Dtos;
using asp.net_web_api.Helpers;
using asp.net_web_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace asp.net_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly FoodShopDbContext context;

        public UsersController(FoodShopDbContext context)
        {
            this.context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = context.Users.SingleOrDefault(user => user.Username == request.Username);
            if (user == null) 
                return Unauthorized("User not found");

            string hashed = PasswordHasher.Md5Hash(request.Password);
            if (user.Password != hashed)
                return Unauthorized("Invalid password");

            var token = CreateJwt(user);

            return Ok(new {token});
        }

        private string CreateJwt(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Memberno.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678901234567890123456789012"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: claims, signingCredentials: creds, expires: DateTime.Now.AddDays(1));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMyProfile()
        {
            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized("User is not authenticated");

            var user = context.Users
                .SingleOrDefault(u => u.Username == username);

            if (user == null)
                return NotFound("User not found");

            // Optional: map to a DTO to avoid exposing sensitive data
            var userDto = new
            {
                user.Memberno,
                user.Username,
                user.Forename,
                user.Surname,
                user.Email,
                user.Street,
                user.Town,
                user.Postcode,
                user.Category
            };

            return Ok(userDto);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            if (newUser == null)
                return BadRequest("Invalid user data");

            if (context.Users.Any(u => u.Username == newUser.Username))
                return BadRequest("Username already exists");

            newUser.Password = PasswordHasher.Md5Hash(newUser.Password);

            context.Users.Add(newUser);
            context.SaveChanges();

            return Ok("User registered successfully");
        }
    }
}
