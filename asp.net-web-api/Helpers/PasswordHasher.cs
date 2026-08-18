using System.Security.Cryptography;
using System.Text;

namespace asp.net_web_api.Helpers
{
    public class PasswordHasher
    {
        public static string Md5Hash(string password)
        {
            var md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes).ToLower(); 
        }
    }
}
