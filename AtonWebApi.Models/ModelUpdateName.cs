using AtonWebApi.Attributes;
using System.ComponentModel.DataAnnotations;


namespace AtonWebApi.Models
{
    public class ModelUpdateName
    {
        public string Login { get; set; }
        public string Password { get; set; }
        [OnlyLatinLettersOrNumbers]
        public string UserLogin { get; set; }
        [OnlyLetters]
        [StringLength(50, MinimumLength = 3)]
        public string NewUserName { get; set; }
    }
}
