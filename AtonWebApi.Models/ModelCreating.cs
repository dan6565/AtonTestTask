
using AtonWebApi.Dto;

namespace AtonWebApi.Models
{
    public class ModelCreating
    {      
        public string Login { get; set; }
        public string Password { get; set; }
        public UserDto User { get; set; }
    }
}
