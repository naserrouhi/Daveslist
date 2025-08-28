using System.ComponentModel.DataAnnotations;

namespace Daveslist.Application.Users.Dtos;

public class RegisterDto
{
    [Required(ErrorMessage = "First name is required")]
    [RegularExpression("^[a-zA-Z\u0600-\u06FF]+$", ErrorMessage = "First name can only contain Persian or English letters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [RegularExpression("^[a-zA-Z\u0600-\u06FF]+$", ErrorMessage = "Last name can only contain Persian or English letters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;

    public string? RecommendedBy { get; set; }
}
