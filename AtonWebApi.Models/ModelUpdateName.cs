using AtonWebApi.Attributes;
using System.ComponentModel.DataAnnotations;


namespace AtonWebApi.Models
{
    public class ModelUpdateName
    {
        public string Token { get; set; }
        public string UserLogin { get; set; }
        [OnlyLetters]
        [StringLength(50, MinimumLength = 3)]
        public string NewUserName { get; set; }
    }
}
