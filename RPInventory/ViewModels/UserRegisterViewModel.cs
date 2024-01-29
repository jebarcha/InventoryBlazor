using RPInventory.Models;
using System.ComponentModel.DataAnnotations;

namespace RPInventory.ViewModels;

public class UserRegisterViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "The Name of user is required.")]
    [MinLength(5, ErrorMessage = "The name of the user should be greater or equal to 2 characters."),
    MaxLength(50, ErrorMessage = "The name of the user should not be greater than 25 characters.")]
    public string Name { get; set; }
    public string Lastname { get; set; }
    
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(5, ErrorMessage = "Username should be greater or equal to 5 characters."),
     MaxLength(20, ErrorMessage = "Username should not be greater than 20 characters.")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(5, ErrorMessage = "Password should be greater or equal to 5 characters."), MaxLength(20, ErrorMessage = "Password should not be greater than 20 characters.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Password Confirmation is required")]
    [MinLength(5, ErrorMessage = "Password Confirmation should be greater or equal to 5 characters."), MaxLength(20, ErrorMessage = "Password should not be greater than 20 characters.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and Confirmation Password does not match.")]
    [Display(Name="Confirm Password")]
    public string PasswordConfirm { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    public string CelPhone { get; set; }
    
    [Required(ErrorMessage = "Profile is required.")]
    [Display(Name = "Profile")]
    public int ProfileId { get; set; }    
}
