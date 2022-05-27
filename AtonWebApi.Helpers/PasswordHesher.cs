using System.Security.Cryptography;
using System.Text;
namespace AtonWebApi.Helpers
{
    public static class PasswordHesher
    {
        public static string GetHashPassword(string password, string secretKey)
        {
            var bytePassword =Encoding.Unicode.GetBytes(password);
            var byteSault = Encoding.Unicode.GetBytes(secretKey);
            using var hmac = new HMACSHA256();
            hmac.Key = byteSault;
            var hash = hmac.ComputeHash(bytePassword);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}