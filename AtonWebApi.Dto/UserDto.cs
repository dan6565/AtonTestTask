using AtonWebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AtonWebApi.Dto
{
    public class UserDto
    {
        [OnlyLatinLettersOrNumbers]
        [StringLength(50, MinimumLength = 3)]
        public string Login { get; set; }
        [OnlyLatinLettersOrNumbers]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [OnlyLetters]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        [Gender]
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
    }
}