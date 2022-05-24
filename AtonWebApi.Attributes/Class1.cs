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
                if (!char.IsLetter(letter))
                {
                    ErrorMessage = "This field can consist only from latin letters";
                    return false;
                }
            }
            return true;
        }
    }
}