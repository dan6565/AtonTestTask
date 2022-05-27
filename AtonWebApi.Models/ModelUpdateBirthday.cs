
namespace AtonWebApi.Models
{
    public class ModelUpdateBirthday
    {
         public string Token { get; set; }      
        public string UserLogin { get; set; }     
        public DateTime NewBirthday { get; set; }
    }
}
