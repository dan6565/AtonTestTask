using AtonWebApi.Attributes;

namespace AtonWebApi.Models
{
    public class ModelUpdateGender
    {
        public string Token { get; set; }
        public string UserLogin { get; set; }
        [Gender]
        public int NewGender { get; set; }
    }
}
