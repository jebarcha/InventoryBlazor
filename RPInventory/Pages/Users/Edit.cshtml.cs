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

public class EditModel : PageModel
{
    private readonly InventoryContext _context;
    private readonly INotyfService _serviceNotification;
    private readonly UserFactory _userFactory;

    public EditModel(InventoryContext context, INotyfService serviceNotification, UserFactory userFactory)
    {
        _context = context;
        _serviceNotification = serviceNotification;
        _userFactory = userFactory;
    }

    [BindProperty]
    public UserEditViewModel User { get; set; }
    public SelectList Profiles { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotification.Warning($"User Id Should not be null");
            return NotFound();
        }

        var userDb = await _context.Users
                                    //.Include(p => p.Marca)
                                    .FirstOrDefaultAsync(m => m.Id == id);
        if (userDb == null)
        {
            _serviceNotification.Warning($"User Id Not Found");
            return NotFound();
        }

        Profiles = new SelectList(_context.Profiles.AsNoTracking(), "Id", "Name");
        User = _userFactory.CreateUserEdit(userDb);

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _serviceNotification.Error($"Fix the problems to edit the product {User.Name}");
            Profiles = new SelectList(_context.Profiles.AsNoTracking(), "Id", "Name");
            return Page();
        }

        var existUserDb = _context.Users.Any(u => u.Username.ToLower().Trim() == User.Username.ToLower().Trim()
                                                   && u.Id != User.Id);

        if (existUserDb)
        {
            Profiles = new SelectList(_context.Profiles.AsNoTracking(), "Id", "Name");
            _serviceNotification.Warning($"Username {User.Username} already exists");
            return Page();
        }

        var userDb = await _context.Users.FindAsync(User.Id);
        _userFactory.UpdateUserData(User, userDb);

        if (Request.Form.Files.Count > 0)
        {
            IFormFile file = Request.Form.Files.FirstOrDefault();
            using var dataStream = new MemoryStream();
            await file.CopyToAsync(dataStream);
            userDb.Photo = dataStream.ToArray();
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(User.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        _serviceNotification.Success($"User updated successfully {User.Username}");
        return RedirectToPage("./Index");
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }

}
