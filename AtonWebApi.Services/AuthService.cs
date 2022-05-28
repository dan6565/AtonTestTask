using AtonTestTask.Interfaces;
using AtonWebApi.Entities;
using AtonWebApi.Helpers;
using AtonWebApi.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AtonWebApi.Services
{
    public class AuthService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsersRepository usersRepository,IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _configuration = configuration;
        }
        public async Task<IBaseResponse> AuthenticateAsync(string login,string password)
        {
            var secretKey = _configuration.GetSection("AppSettings:SecretKey").Value;
            var hashPassword = PasswordHesher.GetHashPassword(password,secretKey);
            User user = null;
            try
            {
                user = await _usersRepository.GetUserAsync(login, hashPassword);
            }
            catch(Exception ex)
            {
                return new BaseResponse()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.DataBaseError
                };
            }
            

            if (user == null)
            {
                return new BaseResponse()
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Invalid login or password"
                };
            }
            if (user.RevokedBy != null)
            {
                return new BaseResponse()
                {
                    StatusCode = StatusCode.AccessDenied,
                    Description = "The rights of the user performing the operation have been revoked"
                };
            }            
            var token = CreateToken(secretKey,user);
            return new BaseResponse<string>()
            {
                StatusCode = StatusCode.Ok,
                Description = "Success",
                Data = token
            };
        }
        public IBaseResponse CheckAccessAdminOrPerformingUser(string token,string userLogin)
        {
            if (!IsValidToken(token))
            {
                return new BaseResponse()
                {
                    StatusCode = StatusCode.AccessDenied,
                    Description = "Invalid token"
                }; 
            }
            if (!IsAdmin(token))
            {
                var userLoginFromToken = GetUserLogin(token);
                if (userLoginFromToken != userLogin)
                {
                    return new BaseResponse()
                    {
                        StatusCode = StatusCode.AccessDenied,
                        Description = "Only the admin or the user himself can do this"
                    };
                }
            }
            return new BaseResponse() { StatusCode = StatusCode.Ok };
        }
        public bool IsAdmin(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ReadJwtToken(token).Claims;
            var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            return role=="Admin";
        }
        public string GetUserLogin(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ReadJwtToken(token).Claims;
            var login = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            return login;
        }
        public IBaseResponse VerifyAdmin(string token)
        {
            if (!IsValidToken(token))
            {
                return new BaseResponse()
                {
                    Description = "Invalid token",
                    StatusCode = StatusCode.AccessDenied
                };
            }            
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ReadJwtToken(token).Claims;
            var role = claims.FirstOrDefault(x=>x.Type== ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                return new BaseResponse()
                {
                    Description = "Only Admin can do this",
                    StatusCode = StatusCode.AccessDenied
                };
            }
            return new BaseResponse() { StatusCode = StatusCode.Ok };
        }
   
        public bool IsValidToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler(); 
           
            var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:SecretKey").Value));                
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secretKey
                }, out SecurityToken validatedToken);
            }
            catch(Exception) 
            {
                return false;
            }
            return true;  
           
        }
        public string CreateToken(string secretKey,User user)
        {
            var symmetricKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
            var credentionals = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role,user.Admin?"Admin":"User")
            };
            var jsonSecTok = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentionals
                );
            return new JwtSecurityTokenHandler().WriteToken(jsonSecTok);
        }
        
        
    }
}
