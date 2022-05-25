using AtonWebApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AtonWebApi.Models
{
    public class ModelUpdatePassword
    {
        public string Login { get; set; }
        public string Password { get; set; }
        [OnlyLatinLettersOrNumbers]
        public string UserLogin { get; set; }
        [OnlyLatinLettersOrNumbers]
        [StringLength(50, MinimumLength = 6)]
        public string NewUserPassword { get; set; }
    }
}
