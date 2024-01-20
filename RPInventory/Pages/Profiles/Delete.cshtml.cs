using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Profiles
{
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
        public Profile Profile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                _serviceNotification.Warning($"Profile Id Should not be null");
                return NotFound();
            }

            Profile = await _context.Profiles.FirstOrDefaultAsync(m => m.Id == id);

            if (Profile == null)
            {
                _serviceNotification.Warning($"Profile Id Not Found");
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

            Profile = await _context.Profiles.FindAsync(id);

            if (Profile != null)
            {
                _context.Profiles.Remove(Profile);
                await _context.SaveChangesAsync();
            }

            _serviceNotification.Success($"Profile deleted successfully {Profile.Name}");
            return RedirectToPage("./Index");
        }
    }
}
