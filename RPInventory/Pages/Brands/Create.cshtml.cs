using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Brands;

public class CreateModel : PageModel
{
    private readonly InventoryContext _context;
    private readonly INotyfService _serviceNotification;

    public CreateModel(InventoryContext context, INotyfService serviceNotification)
    {
        _context = context;
        _serviceNotification = serviceNotification;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Brand Brand { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _serviceNotification.Error($"There's an error. Please fix the problems to create the brand {Brand.Name}");
            return Page();
        }

        var existBrandInDb = _context.Brands.Any(u => u.Name.ToLower().Trim() == Brand.Name.ToLower().Trim());
        if (existBrandInDb)
        {
            _serviceNotification.Warning($"Brand Name {Brand.Name} already exists");
            return Page();
        }

        _context.Brands.Add(Brand);
        await _context.SaveChangesAsync();

        _serviceNotification.Success($"Brand created successfully {Brand.Name}");

        return RedirectToPage("./Index");
    }
}
