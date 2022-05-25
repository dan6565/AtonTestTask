using AtonWebApi.Attributes;


namespace AtonWebApi.Models
{
    public class ModelUpdateBirthday
    {
         public string Login { get; set; }
        public string Password { get; set; }
        [OnlyLatinLettersOrNumbers]
        public string UserLogin { get; set; }     
        public DateTime NewBirthday { get; set; }
    }
}
