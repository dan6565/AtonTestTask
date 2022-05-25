

using AtonWebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AtonWebApi.Models
{
    public class UserDto
    {
        [OnlyLatinLettersOrNumbersAttribute]
        [StringLength(50, MinimumLength = 3)]
        public string Login { get; set; }
        [OnlyLatinLettersOrNumbersAttribute]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
        [OnlyLetters]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
    }
}