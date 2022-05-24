using System.ComponentModel.DataAnnotations;


namespace AtonWebApi.Attributes
{
    public class OnlyLatinLettersOrNumbersAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var str = (string)value;
            foreach (var symbol in str)
            {
                if (!(symbol>='A'&&symbol<='z'||symbol>='0'&&symbol<='9'))
                {
                    ErrorMessage = "This field can consist only latin letters or numbers";
                    return false;
                }
            }
            return true;
        }
    }
}
