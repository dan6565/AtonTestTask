using AtonWebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtonWebApi.Models
{
    public class ModelForUpdateBirthday
    {
         public string Login { get; set; }
        public string Password { get; set; }
        [OnlyLatinLettersOrNumbers]
        public string UserLogin { get; set; }     
        public DateTime NewBirthday { get; set; }
    }
}
