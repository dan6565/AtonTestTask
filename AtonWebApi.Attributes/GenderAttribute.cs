using System.ComponentModel.DataAnnotations;

namespace AtonWebApi.Attributes
{
    public class GenderAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            int ? valueAsInt = (int?)value;
            if (valueAsInt == null)
            {
                return false;
            }
            if (valueAsInt >= 0 && valueAsInt <= 2) return true;

            ErrorMessage = "Gender: 0 - Female, 1 - Male, 2 - Unknown";
            return false;
        }
    }
}
