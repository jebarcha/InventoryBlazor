using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Users;

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
    public User User { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotification.Warning($"User Id Should not be null");
            return NotFound();
        }

        User = await _context.Users
            .Include(p => p.Profile).FirstOrDefaultAsync(m => m.Id == id);

        if (User == null)
        {
            _serviceNotification.Warning($"User Not Found");
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

        User = await _context.Users.FindAsync(id);

        if (User != null)
        {
            _context.Users.Remove(User);
            await _context.SaveChangesAsync();
        }

        _serviceNotification.Success($"User deleted successfully {User.Username}");

        return RedirectToPage("./Index");
    }
}
