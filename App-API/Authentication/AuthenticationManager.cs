using App_API.Models;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App_API.Authentication
{
    public interface IAuthenticationManager
    {
        IDictionary<string, string> Authenticate(string phone, string password);
    }
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly string tokenKey;
        public AuthenticationManager(string tokenKey)
        {
            this.tokenKey = tokenKey;
        }
        public IDictionary<string, string> Authenticate(string name, string password)
        {
            FoodDbContext db = new FoodDbContext();
            UserDetails cust = db.UserDetails.Where(log => log.LoginId == name && log.Password == password).FirstOrDefault();
            if (cust == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, name)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Dictionary<string, string>
            {
                { "Loginid",cust.LoginId },
                { "username", cust.UserName },
                {"useraddress",cust.Address },
                { "Phone", cust.Phone.ToString()},

                { "token",tokenHandler.WriteToken(token) }
            };
        }
    }
}
