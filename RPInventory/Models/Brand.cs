using System.ComponentModel.DataAnnotations;

namespace RPInventory.Models;

public class Brand
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "The brand is required")]
    [MinLength(5, ErrorMessage = "The brand must be greater or equal to 5 characters")]
    [MaxLength(50, ErrorMessage = "The brand must be smaller or equal to 50 characters")]
    [Display(Name = "Brand Name")]
    public string Name { get; set; }
    
    public virtual ICollection<Product> Products { get; set; }
}
