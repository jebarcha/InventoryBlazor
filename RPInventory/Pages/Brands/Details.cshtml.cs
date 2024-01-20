using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Brands;

public class DetailsModel : PageModel
{
    private readonly InventoryContext _context;

    public DetailsModel(InventoryContext context)
    {
        _context = context;
    }

    public Brand Brand { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var brand = await _context.Brands
                            .AsNoTracking()
                            .FirstOrDefaultAsync(m => m.Id == id);
        if (brand == null)
        {
            return NotFound();
        }
        else
        {
            Brand = brand;
        }
        return Page();
    }
}
