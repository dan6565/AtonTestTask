
using AtonWebApi.Entities;

namespace AtonTestTask.Interfaces
{
    public interface IUsersRepository
    {
        public Task CreateUserAsync(User user);
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(User user);
        public Task<User> GetUserAsync(string login);
        public Task<User> GetUserAsync(string login,string password);
        public Task<User[]> GetUsersByAgeAsync(int age);
        public Task<User[]> GetActiveUsersAsync();
        public User GetUser(string login);
        public User GetUser(string login, string password);
       
    }
}