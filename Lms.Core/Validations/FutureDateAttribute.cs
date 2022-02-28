using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Validations;

public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        //var baseValidation = base.IsValid(value, validationContext);
        //if (baseValidation != ValidationResult.Success)
        //{
        //    return baseValidation!;
        //}

        var date = (DateTime)value!;
        if (date < DateTime.Now)
        {
            return new ValidationResult("Future date only");
        }


        return ValidationResult.Success!;
    }
}
