using System.ComponentModel.DataAnnotations;

namespace RPInventory.Models;

public class Profile
{
    public int Id { get; set; }
    [Required(ErrorMessage = "The name of the Profile is required.")]
    [MinLength(5, ErrorMessage = "The name of the Profile should be greater or equal to 5 characters."),
    MaxLength(50, ErrorMessage = "The name of the Profile should not be greater than 50 characters.")]
    public string Name { get; set; }
    public virtual ICollection<User> User { get; set; }
}
