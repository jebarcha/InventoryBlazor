using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Profiles
{
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _serviceNotification.Error($"Fix the problems to edit the profile {Profile.Name}");
                return Page();
            }

            var existsProfileDb = _context.Profiles.Any(u => u.Name.ToLower().Trim() == Profile.Name.ToLower().Trim()
                                                            && u.Id != Profile.Id);

            if (existsProfileDb)
            {
                _serviceNotification.Warning($"Profile Name {Profile.Name} already exists");
                return Page();
            }

            _context.Attach(Profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(Profile.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _serviceNotification.Success($"Profile updated successfully {Profile.Name}");
            return RedirectToPage("./Index");
        }

        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.Id == id);
        }
    }
}
