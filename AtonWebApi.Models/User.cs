using AtonWebApi.Dto;
using System.ComponentModel.DataAnnotations;

namespace AtonWebApi.Models
{
    public class User
    {
       [Key]
        public Guid Guid { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = String.Empty;
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; } = String.Empty;
        public DateTime RevokedOn { get; set; }
        public string RevokedBy { get; set; } = String.Empty;
        public User() { }
        public User(UserDto userDto,string creator)
        {
            Login = userDto.Login;
            Password = userDto.Password;
            Name = userDto.Name;
            Gender = userDto.Gender;
            Birthday = userDto.Birthday;
            Admin = userDto.Admin;
        }
    }
   
}