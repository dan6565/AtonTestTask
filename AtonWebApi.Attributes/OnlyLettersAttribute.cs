using System.ComponentModel.DataAnnotations;

namespace AtonWebApi.Attributes
{
    public class OnlyLettersAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var str = (string)value;
            foreach(var letter in str)
            {
                if (!(letter >= 'A' && letter <= 'z'|| letter >= 'А' && letter <= 'я'))
                {
                    ErrorMessage = "This field can consist only russin or latin letters";
                    return false;
                }
            }
            return true;
        }
    }
  
}