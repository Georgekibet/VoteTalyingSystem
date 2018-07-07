using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vts.Shared.Services
{
    public static class ValidationExtensions
    {
        public static ValidationResultInfo BasicValidation<T>(this T itemToValidate)
        {
            ValidationContext vt = new ValidationContext(itemToValidate, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            Validator.TryValidateObject(itemToValidate, vt, results, true);

            return new ValidationResultInfo { Results = results };
        }

        public const string EmailRegex = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$";
    }
}
