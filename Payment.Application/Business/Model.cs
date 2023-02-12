using System;
using System.ComponentModel.DataAnnotations;

namespace Payment.Application.Business
{
	public class Model : IValidatableObject
	{
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new ValidationResult[] { };
        }

        public Result Validate()
        {
            var errors = new List<ValidationResult>();
            var ctx = new ValidationContext(this);
            var isValid = Validator.TryValidateObject(this, ctx, errors, true);
            return new Result(isValid, errors);
        }

        public class Result
        {
            public bool IsValid { get; }
            public IEnumerable<ValidationResult> Errors { get; }

            public Result(bool isValid, IEnumerable<ValidationResult> errors)
            {
                IsValid = isValid;
                Errors = errors;
            }
        }
    }
}

