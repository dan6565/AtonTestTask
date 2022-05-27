using AtonTestTask.Interfaces;
using AtonWebApi.Dto;
using AtonWebApi.Entities;
using AtonWebApi.Helpers;
using AtonWebApi.Response;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AtonWebApi.Services
{
    public class UsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public UsersService(IUsersRepository usersRepository,AuthService authService,IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _authService = authService;
            _configuration = configuration;
        }
        public async Task<IBaseResponse> CreateUserAsync(string creator, UserDto userDto)
        {
            bool exists;
            try
            {
                exists = await _usersRepository.UserExistsAsync(userDto.Login);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            if (exists)
            {
                return new BaseResponse()
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "User with this login already exists"
                };
            }
            var sercretKey = _configuration.GetSection("AppSettings:SecretKey").Value;
            var hashUserPassword = PasswordHesher.GetHashPassword(userDto.Password,sercretKey);            
            var newUser = new User(userDto, creator, hashUserPassword);
            try
            {
                await _usersRepository.CreateUserAsync(newUser);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }            

            return new BaseResponse() { StatusCode = StatusCode.Ok,Description="User has been successfully added" };
        }
        public async Task<IBaseResponse> UpdateLoginAsync(string modifiedBy, string userLogin,string newUserLogin)
        {
            User user = null;
            try
            {
                bool userExists = _usersRepository.TryGetUserByLogin(userLogin, out user);
                if (!userExists)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "User with this doesn't exist"
                    };
                }
                userExists =await _usersRepository.UserExistsAsync(newUserLogin);
                if (userExists)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.BadRequest,
                        Description = "User with this login already exists"
                    };
                }
                user.Login = newUserLogin;
                UpdateModifyFields(user, modifiedBy);
                await _usersRepository.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            var sercretKey = _configuration.GetSection("AppSettings:SecretKey").Value;
            var newToken = _authService.CreateToken(sercretKey, user);
            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = $"Login has been successfully changed\nNew Token: {newToken}" };
        }

        public async Task<IBaseResponse> UpdatePasswordAsync(string modifiedBy, string userLogin, string newUserPassword)
        {
            try
            {
                bool userExists = _usersRepository.TryGetUserByLogin(userLogin, out User user);
                if (!userExists)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "User with this doesn't exist"
                    };
                }
                user.Password = newUserPassword;
                UpdateModifyFields(user, modifiedBy);
                await _usersRepository.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            
            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "Password has been successfully changed" };            
        }
        public async Task<IBaseResponse> UpdateNameAsync(string modifiedBy, string userLogin,string newUserName)
        {

            try
            {
                bool userExists = _usersRepository.TryGetUserByLogin(userLogin, out User user);
                if (!userExists)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "User with this doesn't exist"
                    };
                }
                user.Name = newUserName;
                UpdateModifyFields(user, modifiedBy);
                await _usersRepository.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "Name has been successfully changed" };            
        }
        public async Task<IBaseResponse> UpdateGenderAsync(string modifiedBy, string userLogin,int gender)
        {

            try
            {
                bool userExists = _usersRepository.TryGetUserByLogin(userLogin, out User user);
                if (!userExists)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "User with this doesn't exist"
                    };
                }
                user.Gender = gender;
                UpdateModifyFields(user, modifiedBy);
                await _usersRepository.UpdateUserAsync(user);

            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
           
            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "Gender has been successfully changed" };
        }
        public async Task<IBaseResponse> UpdateBirthdayAsync(string modifiedBy, string userLogin,DateTime newUserBirthday)
        {
            try
            {
                bool userExists = _usersRepository.TryGetUserByLogin(userLogin, out User user);
                if (!userExists)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "User with this doesn't exist"
                    };
                }
                user.Birthday = newUserBirthday;
                UpdateModifyFields(user, modifiedBy);
                await _usersRepository.UpdateUserAsync(user);

            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "Birthday has been successfully changed" };           
        }
        public async Task<IBaseResponse> GetActiveUsersAsync()
        {
            User[] users;
            try
            {
                users = await _usersRepository.GetActiveUsersAsync();
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            return new BaseResponse<User[]>()
            {
                StatusCode = StatusCode.Ok,
                Data = users
            };
        }
        public IBaseResponse GetUserDataForAdmin(string userLogin)
        {
            User user = null;
            bool userExists;
            try
            {
                userExists = _usersRepository.TryGetUserByLogin(userLogin, out user);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            if (!userExists)
            {
                return new BaseResponse()
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "User with this login don't found"
                };
            }
            return new BaseResponse<User>()
            {
                StatusCode = StatusCode.Ok,
                Data = user
            };
        }
        public  async Task<IBaseResponse> GetUserDataAsync(string login)
        {
            User user = null;            
            try
            {
                 user = await _usersRepository.GetUserAsync(login);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            return new BaseResponse<User>()
            {
                StatusCode= StatusCode.Ok,
                Data = user
            };
        }
        public async Task<IBaseResponse> GetUsersByAgeAsync(int age)
        {
            User[] users;
            try
            {
                users = await _usersRepository.GetUsersByAgeAsync(age);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            return new BaseResponse<User[]>()
            {
                StatusCode = StatusCode.Ok,
                Data = users
            };
        }
        public async Task<IBaseResponse> DeleteUserAsync(string userLogin)
        {
            try
            {
                bool userExists = _usersRepository.TryGetUserByLogin(userLogin, out User user);
                if (!userExists)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "User with this doesn't exist"
                    };
                }
                await _usersRepository.DeleteUserAsync(user);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            
            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "User has been successfully deleted" };
        }
        public async Task<IBaseResponse> RevokeUserAsync(string revokedBy,string userLogin)
        {       
            try
            {
                bool userExists = _usersRepository.TryGetUserByLogin(userLogin, out User user);
                if (!userExists)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "User with this doesn't exist"
                    };
                }
                user.RevokedBy = revokedBy;
                user.RevokedOn = DateTime.Now;
                await _usersRepository.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            
            
            return new BaseResponse()
            {
                StatusCode = StatusCode.Ok,
                Description = "User has been successfully revoked"
            };
        }
        public async Task<IBaseResponse> RecoveryUserAsync( string userLogin)
        {

            try
            {
                bool userExists = _usersRepository.TryGetUserByLogin(userLogin, out User user);
                if (!userExists)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "User with this doesn't exist"
                    };
                }
                user.RevokedBy = null;
                user.RevokedOn = null;
                await _usersRepository.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }            
            return new BaseResponse()
            {
                StatusCode = StatusCode.Ok,
                Description = "User has been successfully recovered"
            };
        }
        private void UpdateModifyFields(User user,string modifiedBy)
        {
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.Now;
        }  
       
    }
}