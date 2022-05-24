

using AtonWebApi.Attributes;

namespace AtonWebApi.Models
{
    public class UserDto
    {
        [OnlyLatinLettersOrNumbersAttribute]
        public string Login { get; set; }
        [OnlyLatinLettersOrNumbersAttribute]
        public string Password { get; set; }
        [OnlyLetters]
        public string Name { get; set; }
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
    }
}