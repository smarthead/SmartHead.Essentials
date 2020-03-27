using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SmartHead.Essentials.Application.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (!(value is IFormFile file))
                return new ValidationResult(typeof(IFormFile) + " is required.");

            var extension = Path.GetExtension(file.FileName);

            return !_extensions.Contains(extension.ToLower())
                ? new ValidationResult(GetErrorMessage(extension))
                : ValidationResult.Success;
        }

        public virtual string GetErrorMessage(string extension) => $"The {extension} extension is not allowed!";
    }
}
