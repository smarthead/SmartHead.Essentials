using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SmartHead.Essentials.Application.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize) => _maxFileSize = maxFileSize;

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (!(value is IFormFile file))
                return new ValidationResult(GetRequiredMessage());

            return file.Length > _maxFileSize ? new ValidationResult(GetErrorMessage()) : ValidationResult.Success;
        }

        public virtual string GetRequiredMessage() => typeof(IFormFile) + " is required.";
        public virtual string GetErrorMessage() => $"Maximum allowed file size is {_maxFileSize} bytes.";
    }
}