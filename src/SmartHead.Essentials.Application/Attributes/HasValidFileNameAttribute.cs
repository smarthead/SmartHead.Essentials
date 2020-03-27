using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace SmartHead.Essentials.Application.Attributes
{
    public class HasValidFileNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (!(value is IFormFile file))
                return new ValidationResult(typeof(IFormFile) + " is required.");

            var isValid = !string.IsNullOrEmpty(file.FileName) &&
                          file.FileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;

            return !isValid
                ? new ValidationResult(GetErrorMessage())
                : ValidationResult.Success;
        }

        public virtual string GetErrorMessage() =>
            "The file name contains invalid characters! Rename the file and try again.";
    }
}