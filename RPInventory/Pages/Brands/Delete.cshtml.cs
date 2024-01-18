using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Brands;

public class DeleteModel : PageModel
{
    private readonly InventoryContext _context;
    private readonly INotyfService _serviceNotification;

    public DeleteModel(InventoryContext context, INotyfService serviceNotification)
    {
        _context = context;
        _serviceNotification = serviceNotification;
    }

    [BindProperty]
    public Brand Brand { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotification.Warning($"Brand Id Should not be null");
            return NotFound();
        }

        var brand = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);

        if (brand == null)
        {
            _serviceNotification.Warning($"Brand Id Not Found");
            return NotFound();
        }
        else
        {
            Brand = brand;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotification.Warning($"Brand Id Not Found");
            return NotFound();
        }

        var brand = await _context.Brands.FindAsync(id);
        if (brand != null)
        {
            Brand = brand;
            _context.Brands.Remove(Brand);
            await _context.SaveChangesAsync();
        }

        _serviceNotification.Success($"Brand deleted successfully {Brand.Name}");
        return RedirectToPage("./Index");
    }
}
