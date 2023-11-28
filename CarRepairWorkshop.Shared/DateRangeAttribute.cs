using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class DateRangeAttribute : ValidationAttribute
{
    private readonly DateTime _minimumDate = new DateTime(1900, 1, 1);

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateValue)
        {
            if (dateValue < _minimumDate || dateValue >= DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success;
    }
}
