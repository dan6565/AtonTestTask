using AtonTestTask.Data.Repositories;

namespace AtonWebApi.Services
{
    public class UsersServices
    {
        private readonly UsersRepository _repository;
        public UsersServices(UsersRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> UserExists(string login)
        {
            var user = await _repository.GetUserAsync(login);
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}