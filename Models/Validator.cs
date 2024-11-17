using System.ComponentModel.DataAnnotations;

namespace Coursera.Models
{
    public class Validator:ValidationAttribute
    {
        public readonly string[] fileExtension = { ".jpg",".jpeg",".png" };
        public readonly string[] fileTypes = { "image/jpeg","image/png","image/jpg"};
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var extension=Path.GetExtension(file.FileName);
               
                if(!fileExtension.Contains(extension))
                {
                    return new ValidationResult($"File type should be {string.Join(',',fileExtension)} format");
                }
                if(!fileTypes.Contains(file.ContentType))
                {
                    return new ValidationResult($"File content type should be {string.Join(',',fileTypes)} type");
                }
            }
            else
            {
                return new ValidationResult("File is required");
            }
            return ValidationResult.Success;

        }
    }
}
