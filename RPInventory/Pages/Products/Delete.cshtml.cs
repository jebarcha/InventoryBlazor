using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Products;

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
    public Product Product { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotification.Warning($"Product Id Should not be null");
            return NotFound();
        }

        Product = await _context.Products
            .Include(p => p.Brand).FirstOrDefaultAsync(m => m.Id == id);

        if (Product == null)
        {
            _serviceNotification.Warning($"Product Id Not Found");
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Product = await _context.Products.FindAsync(id);

        if (Product != null)
        {
            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();
        }

        _serviceNotification.Success($"Product deleted successfully {Product.Name}");

        return RedirectToPage("./Index");
    }
}
