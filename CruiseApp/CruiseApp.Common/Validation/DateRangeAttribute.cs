using System.ComponentModel.DataAnnotations;

namespace CruiseApp.Common.Validation
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;

        public DateRangeAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var startDateProp = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            if (startDateProp == null) return ValidationResult.Success;

            var startValue = startDateProp.GetValue(validationContext.ObjectInstance) as DateOnly?;
            var endValue = value as DateOnly?;

            if (startValue.HasValue && endValue.HasValue && endValue <= startValue)
                return new ValidationResult(ErrorMessage ?? "End date must be after start date.");

            return ValidationResult.Success;
        }
    }
}
