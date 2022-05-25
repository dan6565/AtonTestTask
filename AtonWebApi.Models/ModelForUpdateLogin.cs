using AtonWebApi.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtonWebApi.Models
{
    public class ModelForUpdateLogin
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
