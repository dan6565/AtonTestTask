using AtonWebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AtonWebApi.Models
{
    public class User
    {
       [Key]
        public Guid Guid { get; set; }
        [StringLength(50,MinimumLength =3)]
        public string Login { get; set; }
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        [Gender]
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
        public DateTime? CreatedOn { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? CreatedBy { get; set; } 
        public DateTime? ModifiedOn { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? ModifiedBy { get; set; } 
        public DateTime? RevokedOn { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? RevokedBy { get; set; }
        public User() { }
        public User(UserDto userDto,string creator)
        {
            Login = userDto.Login;
            Password = userDto.Password;
            Name = userDto.Name;
            Gender = userDto.Gender;
            Birthday = userDto.Birthday;
            Admin = userDto.Admin;
            CreatedBy = creator;
            CreatedOn = DateTime.Now;
        }
    }
   
}