using AtonTestTask.Data.Repositories;
using AtonWebApi.Models;

namespace AtonWebApi.Data.Initialiser
{
    public class AdminInitializer
    {
        public static async Task InitializeAsync(UsersRepository repository)
        {
            var admin = new User()
            {
                Guid = Guid.NewGuid(),
                Login = "admin",
                Password = "admin",
                Gender = 2,
                Name = String.Empty,
                Birthday = null,
                CreatedOn = DateTime.Now,
                CreatedBy = "default",
                ModifiedBy = String.Empty,
                RevokedBy = String.Empty,
                Admin=true,

            };
            User user = await repository.GetUserAsync(admin.Login);
            if(user==null)
            await repository.CreateUser(admin);            
        }
    }
}
