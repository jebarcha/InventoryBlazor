using System.ComponentModel.DataAnnotations;

namespace RPInventory.Models;

public enum StatusProduct
{
    Deleted = 0,
    Active = 1,
    [Display(Name = "In Process of Activation")]
    InProcessActivation = 2
}
