using System.ComponentModel.DataAnnotations;

public class FileExtensionAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            // Kiểm tra extension
            var extension = Path.GetExtension(file.FileName).ToLower();
            string[] allowedExtensions = { ".jpg", ".png", ".jpeg", ".gif", ".bmp" };

            // Kiểm tra MIME type
            string[] allowedMimeTypes = { "image/jpeg", "image/png", "image/gif", "image/bmp" };

            if (!allowedExtensions.Contains(extension) || !allowedMimeTypes.Contains(file.ContentType.ToLower()))
            {
                return new ValidationResult("Chỉ cho phép ảnh có đuôi là jpg, png, jpeg, gif và bmp");
            }

            // Kiểm tra kích thước file nếu cần
            if (file.Length > 1 * 1024 * 1024) // 5MB
            {
                return new ValidationResult("Kích thước file không được vượt quá 1MB");
            }
        }
        return ValidationResult.Success;
    }
}