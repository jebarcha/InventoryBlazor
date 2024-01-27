using System.ComponentModel.DataAnnotations;

namespace RPInventory.ViewModels;

public class UserChangePasswordViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "UserName")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(5, ErrorMessage = "Password should be greater or equal to 5 characters."), MaxLength(20, ErrorMessage = "Password should not be greater than 20 characters.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password Confirmation is required")]
    [MinLength(5, ErrorMessage = "Password Confirmation should be greater or equal to 5 characters."), MaxLength(20, ErrorMessage = "Password should not be greater than 20 characters.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and Confirmation Password does not match.")]
    public string PasswordConfirm { get; set; }
}
