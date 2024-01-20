using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Products;

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
        Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
        return Page();
    }

    [BindProperty]
    public Product Product { get; set; }
    public SelectList Brands { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
            _serviceNotification.Error($"There's an error. Please fix the problems to create the producto {Product.Name}");
            return Page();
        }

        var existProductDb = _context.Products.Any(u => u.Name.ToLower().Trim() == Product.Name.ToLower().Trim());
        if (existProductDb)
        {
            Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
            _serviceNotification.Warning($"Product name already exists {Product.Name}");
            return Page();
        }

        _context.Products.Add(Product);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
