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
        
        public async Task CreateUser(User user)
        {
           await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(string login)
        {
            User result = await _dbContext.Users.FirstOrDefaultAsync(x=> x.Login == login);
            if (result != null)
            {
                _dbContext.Remove(result);
            }
        }

        public async Task<List<User>> GetActiveUsers()
        {
            return await _dbContext.Users.Where(x=>x.RevokedOn == default(DateTime)).ToListAsync();
        }

        public async Task<User> GetUserAsync(string login)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x=>x.Login == login);
        }


        public async Task UpdateUser(User user)
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
