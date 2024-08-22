using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class ValidSymbolAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var symbol = value as string;
        if (string.IsNullOrWhiteSpace(symbol) || !Regex.IsMatch(symbol, @"^[A-Z]{1,4}$"))
        {
            return new ValidationResult("Sembol yalnızca 1 ila 4 büyük harf içerebilir.");
        }
        return ValidationResult.Success;
    }
}

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
