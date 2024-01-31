using System.ComponentModel.DataAnnotations;

namespace RPInventarios.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "User name is required.")]
    [Display(Name = "User name")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
