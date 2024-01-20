﻿using System.ComponentModel.DataAnnotations;

namespace RPInventory.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int BrandId { get; set; }
    public virtual Brand Brand { get; set; }
    public decimal Price { get; set; }
    [Display(Name = "Status")]
    [Required(ErrorMessage = "Status of the product is required.")]
    public StatusProduct Status { get; set; } = StatusProduct.Active;
}
