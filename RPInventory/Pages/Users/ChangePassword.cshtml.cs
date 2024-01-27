using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Helpers;
using RPInventory.Models;
using RPInventory.ViewModels;

namespace RPInventory.Pages.Users;

public class ChangePasswordModel : PageModel
{
    private readonly InventoryContext _context;
    private readonly INotyfService _serviceNotification;
    private readonly UserFactory _userFactory;

    public ChangePasswordModel(InventoryContext context, INotyfService serviceNotification, UserFactory userFactory)
    {
        _context = context;
        _serviceNotification = serviceNotification;
        _userFactory = userFactory;
    }

    [BindProperty]
    public UserChangePasswordViewModel User { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotification.Warning($"User Id Should not be null");
            return NotFound();
        }

        var userDb = await _context.Users
                                    .FirstOrDefaultAsync(m => m.Id == id);
        if (userDb == null)
        {
            _serviceNotification.Warning($"User Id Not Found");
            return NotFound();
        }

        User = _userFactory.CreateUserChangePassword(userDb);

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _serviceNotification.Error($"Fix the problems to edit the product {User.Username}");
            return Page();
        }

        var userDb = await _context.Users.FindAsync(User.Id);
        _userFactory.ChangePassword(User, userDb);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
        _serviceNotification.Success($"Password updated successfully {User.Username}");
        return RedirectToPage("./Index");
    }

}
