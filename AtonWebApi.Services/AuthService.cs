using AtonTestTask.Interfaces;
using AtonWebApi.Entities;
using AtonWebApi.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

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
        public async Task<IBaseResponse> Authenticate(string login,string password)
        {
           
            var hashPassword = GetHashPassword(password);
            var user = await _usersRepository.GetUserAsync(login, hashPassword);
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
            var secretKey = _configuration.GetSection("AppSettings:SecretKey").Value;
            var token = CreateToken(secretKey,user);
            return new BaseResponse<string>()
            {
                StatusCode = StatusCode.Ok,
                Description = "Success",
                Data = token
            };
        }
        public void CheckUserToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler(); 
           
           if (tokenHandler.CanReadToken(token))
            {
                var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:SecretKey").Value));
                var jwt = tokenHandler.ReadJwtToken(token);
                List<Claim> claims = jwt.Claims.ToList();
                var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience=false,
                    ValidateIssuer=false,
                    ValidateIssuerSigningKey = true,                   
                    IssuerSigningKey = secretKey
                }, out SecurityToken validatedToken);
               
                
                
               
            }
        }
        private string CreateToken(string secretKey,User user)
        {
            var symmetricKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
            var credentionals = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role,user.Admin?"Admin":"User")
            };
            var jsonSecTok = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentionals
                );
            return new JwtSecurityTokenHandler().WriteToken(jsonSecTok);
        }
        public string GetHashPassword(string password)
        {
            var secretKey = _configuration.GetSection("AppSettings:SecretKey").Value;
            var bytePassword = System.Text.Encoding.Unicode.GetBytes(password);
            var byteSault = System.Text.Encoding.Unicode.GetBytes(secretKey);
            using var hmac = new HMACSHA256();
            hmac.Key = byteSault;
            var hash = hmac.ComputeHash(bytePassword);
            return BitConverter.ToString(hash).Replace("-","").ToLower();
        }
        
    }
}
