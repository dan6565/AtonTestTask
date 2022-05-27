using AtonTestTask.Interfaces;
using AtonWebApi.Dto;
using AtonWebApi.Entities;
using AtonWebApi.Response;
using System.Security.Claims;


namespace AtonWebApi.Services
{
    public class UsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly AuthService _authService;

        public UsersService(IUsersRepository usersRepository,AuthService authService)
        {
            _usersRepository = usersRepository;
            _authService = authService;
        }
        public async Task<IBaseResponse> CreateUser(string token, UserDto userDto)
        {
            _authService.CheckUserToken(token);
            //var hashPassword = _authService.GetHashPassword(password);
            //var response = CheckAdminData(login, hashPassword);
            //if (response.StatusCode != StatusCode.Ok)
            //{
            //    return response;
            //}            
            if(await UserExists(userDto.Login))
            {
                return new BaseResponse()
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "User with this login already exists"
                };
            }
            var hashNewUserPassword = _authService.GetHashPassword(userDto.Password);
            //var newUser = new User(userDto, login, hashNewUserPassword);
            //await _usersRepository.CreateUserAsync(newUser);

            return new BaseResponse() { StatusCode = StatusCode.Ok,Description="User has been successfully added" };
        }
        public async Task<IBaseResponse> UpdateLogin(string login,string password, string userLogin,string newUserLogin)
        {
            var response = CheckInputData(login, password, userLogin);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var user = ((BaseResponse<User>)response).Data;
            if (await UserExists(newUserLogin))
            {
                return new BaseResponse()
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "User with this login already exists"
                };
            }
            user.Login = newUserLogin;
            UpdateModifyFields(user, login);
            await _usersRepository.UpdateUserAsync(user);

            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "Login has been successfully changed" };
        }

        public async Task<IBaseResponse> UpdatePasswordAsync(string login, string password, string userLogin, string newUserPassword)
        {
            var response = CheckInputData(login, password, userLogin);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var user = ((BaseResponse<User>)response).Data;
            user.Password = newUserPassword;
            UpdateModifyFields(user, login);
            await _usersRepository.UpdateUserAsync(user);

            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "Password has been successfully changed" };            
        }
        public async Task<IBaseResponse> UpdateNameAsync(string login,string password,string userLogin,string newUserName)
        {
            var response = CheckInputData(login, password, userLogin);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var user = ((BaseResponse<User>)response).Data;
            user.Name = newUserName;
            UpdateModifyFields(user, login);
            await _usersRepository.UpdateUserAsync(user);

            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "Name has been successfully changed" };
            
        }
        public async Task<IBaseResponse> UpdateGenderAsync(string login,string password,string userLogin,int gender)
        {
            var response = CheckInputData(login, password, userLogin);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var user = ((BaseResponse<User>)response).Data;
            user.Gender = gender;
            UpdateModifyFields(user, login);
            await _usersRepository.UpdateUserAsync(user);

            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "Gender has been successfully changed" };
        }
        public async Task<IBaseResponse> UpdateBirthdayAsync(string login,string password,string userLogin,DateTime newUserBirthday)
        {
            var response =  CheckInputData(login, password, userLogin);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var user = ((BaseResponse<User>)response).Data;
            user.Birthday = newUserBirthday;
            UpdateModifyFields(user, login);
            await _usersRepository.UpdateUserAsync(user);

            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "Birthday has been successfully changed" };
           
        }
        public async Task<IBaseResponse> GetActiveUsersAsync(string login,string password)
        {
            var response = CheckAdminData(login, password);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var users = await _usersRepository.GetActiveUsersAsync();
            return new BaseResponse<User[]>()
            {
                StatusCode = StatusCode.Ok,
                Data = users
            };
        }
        public IBaseResponse GetUserDataForAdmin(string login,string password,string userLogin)
        {
            var response = CheckAdminData(login, password);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            if(!TryGetUserByLogin(userLogin,out User user))
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
        public  IBaseResponse GetUserData(string login,string password)
        {
            var response =  CheckInputData(login, password, login);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var user = ((BaseResponse<User>)response).Data;
            return new BaseResponse<User>()
            {
                StatusCode= StatusCode.Ok,
                Data = user
            };
        }
        public async Task<IBaseResponse> GetUsersByAgeAsync(string adminLogin,string password,int age)
        {
            var response = CheckAdminData(adminLogin, password);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var users = await _usersRepository.GetUsersByAgeAsync(age);
            return new BaseResponse<User[]>()
            {
                StatusCode = StatusCode.Ok,
                Data = users
            };
        }
        public async Task<IBaseResponse> DeleteUserAsync(string adminLogin,string password,string userLogin)
        {
            var response = CheckDataForOnlyAdminOperation(adminLogin, password, userLogin);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var user = ((BaseResponse<User>)response).Data;
            await _usersRepository.DeleteUserAsync(user);
            return new BaseResponse() { StatusCode = StatusCode.Ok, Description = "User has been successfully deleted" };
        }
        public async Task<IBaseResponse> RevokeUserAsync(string adminLogin,string password,string userLogin)
        {
            var response = CheckDataForOnlyAdminOperation(adminLogin, password, userLogin);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var user = ((BaseResponse<User>)response).Data;
            user.RevokedBy = adminLogin;
            user.RevokedOn = DateTime.Now;
            await _usersRepository.UpdateUserAsync(user);
            return new BaseResponse()
            {
                StatusCode = StatusCode.Ok,
                Description = "User has been successfully revoked"
            };
        }
        public async Task<IBaseResponse> RecoveryUserAsync(string adminLogin, string password, string userLogin)
        {
            var response = CheckDataForOnlyAdminOperation(adminLogin, password, userLogin);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }
            var user = ((BaseResponse<User>)response).Data;
            user.RevokedBy = null;
            user.RevokedOn = null;
            await _usersRepository.UpdateUserAsync(user);
            return new BaseResponse()
            {
                StatusCode = StatusCode.Ok,
                Description = "User has been successfully recovered"
            };
        }
        private IBaseResponse CheckDataForOnlyAdminOperation(string adminLogin,string password,string userLogin)
        {
            var response = CheckAdminData(adminLogin, password);
            if (response.StatusCode != StatusCode.Ok)
            {
                return response;
            }

            if (!TryGetUserByLogin(userLogin, out User user))
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
        private IBaseResponse CheckAdminData(string login,string password)
        {
            if (!TryGetUserByLoginPassword(login, password, out User user))
            {
                return new BaseResponse()
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Invalid login or password"
                };
            }
            if (!user.Admin)
            {
                return new BaseResponse()
                {
                    StatusCode = StatusCode.AccessDenied,
                    Description = "Only administrator can do this"
                };
            }
            return new BaseResponse() { StatusCode = StatusCode.Ok };
        }
        private  IBaseResponse CheckInputData(string login,string password,string userLogin)
        {
            if (!TryGetUserByLoginPassword(login, password, out User user))
            {
                return new BaseResponse()
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Invalid login or password"
                };
            }

            if (!user.Admin)
            {
                if (login != userLogin)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.AccessDenied,
                        Description = "Only the user or the administrator can make changes to the user's properties"
                    };
                }
                if (IsRevoked(user))
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.AccessDenied,
                        Description = "The rights of the user performing the operation have been revoked"
                    };

                }
            }
            if (login != userLogin)
            {
                if (!TryGetUserByLogin(userLogin, out user))
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = $"User with login '{userLogin}' doesn't exist"
                    };
                }
            }
            return new BaseResponse<User>() { StatusCode = StatusCode.Ok, Data = user };
        } 
        private void UpdateModifyFields(User user,string modifiedBy)
        {
            user.ModifiedBy = modifiedBy;
            user.ModifiedOn = DateTime.Now;
        }
        private bool IsRevoked(User user) => user.RevokedOn != null;
        private async Task<bool> UserExists(string login)
        {
            var user = await _usersRepository.GetUserAsync(login);
            if (user != null)
            {
                return true;
            }
            return false;
        }
        private bool TryGetUserByLoginPassword(string login,string password,out User user)
        {
            user = _usersRepository.GetUser(login, password);

            if (user != null)
            {
                return true;
            }
            return false;
        }
        private bool TryGetUserByLogin(string login, out User user)
        {
            user =  _usersRepository.GetUser(login);

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}