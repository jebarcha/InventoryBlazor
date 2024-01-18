using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Brands;

public class EditModel : PageModel
{
    private readonly InventoryContext _context;
    private readonly INotyfService _serviceNotification;

    public EditModel(InventoryContext context, INotyfService serviceNotification)
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

        var brand =  await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);
        if (brand == null)
        {
            _serviceNotification.Warning($"Brand Id Not Found");
            return NotFound();
        }
        Brand = brand;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _serviceNotification.Error($"Fix the problems to edit the brand {Brand.Name}");
            return Page();
        }

        var existBrandInDb = _context.Brands
            .Any(u => u.Name.ToLower().Trim() == Brand.Name.ToLower().Trim()  && u.Id!=Brand.Id);
        if (existBrandInDb)
        {
            _serviceNotification.Warning($"Brand Name {Brand.Name} already exists");
            return Page();
        }


        _context.Attach(Brand).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BrandExists(Brand.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        _serviceNotification.Success($"Brand updated successfully {Brand.Name}");
        return RedirectToPage("./Index");
    }

    private bool BrandExists(int id)
    {
        return _context.Brands.Any(e => e.Id == id);
    }
}
