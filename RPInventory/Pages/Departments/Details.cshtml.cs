using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Departments;

public class DetailsModel : PageModel
{
    private readonly InventoryContext _context;

    public DetailsModel(InventoryContext context)
    {
        _context = context;
    }

    public Department Department { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Department = await _context.Departments
                                .AsNoTracking()
                                .FirstOrDefaultAsync(m => m.Id == id);

        if (Department == null)
        {
            return NotFound();
        }
        return Page();
    }
}
