using RPInventory.Models;
using System.ComponentModel.DataAnnotations;

namespace RPInventory.ViewModels;

public class UserEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Name of user is required.")]
    [MinLength(5, ErrorMessage = "The name of the user should be greater or equal to 2 characters."),
    MaxLength(50, ErrorMessage = "The name of the user should not be greater than 25 characters.")]
    public string Name { get; set; }
    
    public string Lastname { get; set; }
    
    [Display(Name="User name")]
    public string Username { get; set; }
    
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    public string CelPhone { get; set; }
    
    [Required(ErrorMessage = "Profile is required.")]
    [Display(Name = "Profile")]
    public int ProfileId { get; set; }
    
    public Profile Profile { get; set; }
}