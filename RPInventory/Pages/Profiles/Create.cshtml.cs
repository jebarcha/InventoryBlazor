using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Profiles
{
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
            return Page();
        }

        [BindProperty]
        public Profile Profile { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _serviceNotification.Error($"There's an error. Please fix the problems to create the profile {Profile.Name}");
                return Page();
            }

            var existsProfileDb = _context.Profiles.Any(u => u.Name.ToLower().Trim() == Profile.Name.ToLower().Trim());
            if (existsProfileDb)
            {
                _serviceNotification.Warning($"Profile Name {Profile.Name} already exists");
                return Page();
            }

            _context.Profiles.Add(Profile);
            await _context.SaveChangesAsync();
            _serviceNotification.Success($"Profile created successfully {Profile.Name}");
            return RedirectToPage("./Index");
        }
    }
}
