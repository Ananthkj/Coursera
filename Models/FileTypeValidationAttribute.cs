using System.ComponentModel.DataAnnotations;

namespace Coursera.Models
{
    public class FileTypeValidationAttribute : ValidationAttribute
    {
        
        private readonly string[] _fileExtentions = {".jpg",".jpeg",".png" };
        private readonly string[] _fileTypes = { "image/jpeg","image/png"};
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is IFormFile File)
            {
                var extension=Path.GetExtension(File.FileName);
                if(!_fileExtentions.Contains(extension))
                {
                    return new ValidationResult($"File type should be {string.Join(',', _fileExtentions)} Format..");
                }
                if(!_fileTypes.Contains(File.ContentType))
                {
                    return new ValidationResult("Invalid Content type..");
                }
            }
            else
            {
                return new ValidationResult("File is required..");
            }
            return ValidationResult.Success;
        }

    }
}
    