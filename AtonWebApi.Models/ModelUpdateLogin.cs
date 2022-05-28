using AtonWebApi.Attributes;
using System.ComponentModel.DataAnnotations;


namespace AtonWebApi.Models
{
    public class ModelUpdateLogin
    {
        public string Token { get; set; }           
        public string PreviousUserLogin { get; set; }
        [OnlyLatinLettersOrNumbers]
        [StringLength(50,MinimumLength =3)]
        public string NewUserLogin { get; set; }
    }
}
