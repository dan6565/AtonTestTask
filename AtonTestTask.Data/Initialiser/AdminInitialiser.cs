using AtonTestTask.Interfaces;
using AtonWebApi.Entities;

namespace AtonWebApi.Data.Initialiser
{
    public class AdminInitializer
    {
        public static async Task InitializeAsync(IUsersRepository repository)
        {
            var admin = new User()
            {
                Guid = Guid.NewGuid(),
                Login = "admin",
                Password = "admin",
                Gender = 2,
                Name = "admin",
                Birthday = DateTime.Now,
                CreatedOn = DateTime.Now,
                CreatedBy = "default",
                Admin=true,

            };
            User user = await repository.GetUserAsync(admin.Login);
            if(user==null)
            await repository.CreateUserAsync(admin);            
        }
    }
}
