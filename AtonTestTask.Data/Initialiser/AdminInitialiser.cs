using AtonTestTask.Interfaces;
using AtonWebApi.Entities;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace AtonWebApi.Data.Initialiser
{
    public class AdminInitializer
    {
        public static async Task InitializeAsync(IUsersRepository repository,IConfiguration configuration)
        {
            User user = await repository.GetUserAsync("admin");
            if (user == null)
            {
                var admin = new User()
                {
                    Guid = Guid.NewGuid(),
                    Login = "admin",
                    Password = GetHashPassword("admin", configuration.GetSection("AppSettings:SecretKey").Value),
                    Gender = 2,
                    Name = "admin",
                    Birthday = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    CreatedBy = "default",
                    Admin = true

                };

                await repository.CreateUserAsync(admin);
            }
                         
        }
        private static string GetHashPassword(string password,string secretKey)
        {
            var bytePassword = System.Text.Encoding.Unicode.GetBytes(password);
            var byteSault = System.Text.Encoding.Unicode.GetBytes(secretKey);
            using var hmac = new HMACSHA256();
            hmac.Key = byteSault;
            var hash = hmac.ComputeHash(bytePassword);
            return BitConverter.ToString(hash).Replace("-","").ToLower();
        }
    }
}
