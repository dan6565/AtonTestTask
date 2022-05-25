using AtonWebApi.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtonWebApi.Models
{
    public class ModelForUpdateName
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
