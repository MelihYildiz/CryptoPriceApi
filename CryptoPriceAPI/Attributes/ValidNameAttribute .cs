/* using System.ComponentModel.DataAnnotations;

public class ValidNameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var name = value as string;
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2 || name.Length > 25)
        {
            return new ValidationResult("Ad alanı 2 ila 25 karakter arasında olmalıdır.");
        }

        return ValidationResult.Success;
    }
}
*/