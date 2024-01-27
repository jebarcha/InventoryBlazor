using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Users;

public class DetailsModel : PageModel
{
    private readonly InventoryContext _context;

    public DetailsModel(InventoryContext context)
    {
        _context = context;
    }

    public User User { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        User = await _context.Users
                                    .Include(p => p.Profile)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(m => m.Id == id);

        if (User == null)
        {
            return NotFound();
        }
        return Page();
    }
}
