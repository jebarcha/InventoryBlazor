using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPInventory.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MinLength(5, ErrorMessage = "Name should have 5 or more characters."),
    MaxLength(50, ErrorMessage = "Name should have 50 or less characters.")]
    public string Name { get; set; }

    [Display(Name = "Description")]
    [StringLength(200, MinimumLength = 5,
              ErrorMessage = "Description should contain between 5 and 200 characters.")]
    public string Description { get; set; }
    
    [Display(Name = "Brand")]
    [Required(ErrorMessage = "Brand is required.")]
    public int BrandId { get; set; }
    public virtual Brand Brand { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Display(Name = "Status")]
    [Required(ErrorMessage = "Status of the product is required.")]
    public StatusProduct Status { get; set; } = StatusProduct.Active;

    [Display(Name = "Product Image")]
    public string Image { get; set; }
}
