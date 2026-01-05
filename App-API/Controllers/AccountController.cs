using App_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using App_API.Authentication;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserDetail login)
        {
            AuthenticationManager jWTAuthenticationManager = new AuthenticationManager(_configuration.GetValue<string>("TokenKey"));
            var token = jWTAuthenticationManager.Authenticate(login.LoginId, login.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }

        [HttpPost]
        public async Task<JsonResult> Post([FromBody] UserDetails cst)
        {
            try
            {
                using (FoodDbContext entities = new FoodDbContext())
                {
                    entities.UserDetails.Add(cst);
                    await entities.SaveChangesAsync();
                    return new JsonResult("User registered successfully.");
                }
            }
            catch (Exception)
            {
                return new JsonResult("Error while registering User.");
            }
        }
    }
}
