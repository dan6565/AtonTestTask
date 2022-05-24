

using AtonWebApi.Models;

namespace AtonTestTask.Interfaces
{
    public interface IUserRepository
    {
        public Task CreateUserAsync(User user);
        public Task UpdateUser(User user);
        public Task DeleteUser(string login);
        public Task<User> GetUserAsync(string login);
        public Task<User> GetUserAsync(string login,string password);
        public Task<List<User>> GetActiveUsers();
       
    }
}