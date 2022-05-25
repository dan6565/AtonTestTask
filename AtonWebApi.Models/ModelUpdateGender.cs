using AtonWebApi.Attributes;

namespace AtonWebApi.Models
{
    public class ModelUpdateGender
    {
        public string Login { get; set; }
        public string Password { get; set; }
        [OnlyLatinLettersOrNumbers]
        public string UserLogin { get; set; }
        [Gender]
        public int NewGender { get; set; }
    }
}
