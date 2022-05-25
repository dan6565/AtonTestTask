using AtonWebApi.Attributes;
using System.ComponentModel.DataAnnotations;


namespace AtonWebApi.Models
{
    public class ModelUpdateLogin
    {
        public string Login { get; set; }
        public string Password { get; set; }
        [OnlyLatinLettersOrNumbers]       
        public string PreviousUserLogin { get; set; }
        [OnlyLatinLettersOrNumbers]
        [StringLength(50,MinimumLength =3)]
        public string NewUserLogin { get; set; }
    }
}
