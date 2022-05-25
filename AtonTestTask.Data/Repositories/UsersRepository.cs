using AtonTestTask.Interfaces;
using AtonWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AtonTestTask.Data.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UsersRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task CreateUserAsync(User user)
        {
           await _dbContext.Users.AddAsync(user);
           await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
           _dbContext.Remove(user);
           _dbContext.SaveChanges();            
        }

        public async Task<User[]> GetActiveUsersAsync()
        {
            return await _dbContext.Users.Where(x=>x.RevokedOn == null)
                                         .OrderBy(x=>x.CreatedOn).ToArrayAsync();
        }

        public async Task<User> GetUserAsync(string login)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x=>x.Login == login);
        }

        public Task<User> GetUserAsync(string login, string password)
        {
            return _dbContext.Users.FirstOrDefaultAsync(x=>x.Login==login&& x.Password==password);
        }
        public async Task<User[]> GetUsersByAgeAsync(int age)
        {
            var now = DateTime.Now;
            var users = await _dbContext.Users.Where(x => x.Birthday != null).ToArrayAsync();
            return users.Where(x =>now.Year - x.Birthday.GetValueOrDefault().Year -
                                              ((now.Month * 100 + now.Day) < 
                                              (x.Birthday.GetValueOrDefault().Month * 100 +x.Birthday.GetValueOrDefault().Day)
                                              ? 1 : 0) > age).ToArray();
            
        }
        public async Task UpdateUserAsync(User user)
        {
            User result = await _dbContext.Users.FirstOrDefaultAsync(x => x.Guid == user.Guid);
            if (result != null)
            {
                result = user;
                _dbContext.SaveChanges();
            }
        }
    }
}
