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

public class CreateModel : PageModel
{
    private readonly InventoryContext _context;
    private readonly INotyfService _serviceNotification;
    private readonly UserFactory _userFactory;

    public CreateModel(InventoryContext context, INotyfService serviceNotification,
        UserFactory userFactory)
    {
        _context = context;
        _serviceNotification = serviceNotification;
        _userFactory = userFactory;
    }

    public IActionResult OnGet()
    {
        Profiles = new SelectList(_context.Profiles.AsNoTracking(), "Id", "Name");
        User = new UserRegisterViewModel();
        return Page();
    }

    [BindProperty]
    public UserRegisterViewModel User { get; set; }
    public SelectList Profiles { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Profiles = new SelectList(_context.Profiles.AsNoTracking(), "Id", "Username");
            _serviceNotification.Error($"There's an error. Please fix the problems to create the user {User.Username}");
            return Page();
        }

        var existUserDb = _context.Users.Any(u => u.Username.ToLower().Trim() == User.Username.ToLower().Trim());
        if (existUserDb)
        {
            Profiles = new SelectList(_context.Profiles.AsNoTracking(), "Id", "Username");
            _serviceNotification.Warning($"User name already exists {User.Username}");
            return Page();
        }

        var userAdd = _userFactory.CreateUser(User);

        if (Request.Form.Files.Count > 0)
        {
            IFormFile file = Request.Form.Files.FirstOrDefault();
            using var dataStream = new MemoryStream();
            await file.CopyToAsync(dataStream);
            userAdd.Photo = dataStream.ToArray();
        }

        _context.Users.Add(userAdd);
        await _context.SaveChangesAsync();
        _serviceNotification.Success($"User {User.Username} added successfully");

        return RedirectToPage("./Index");
    }

    public async Task<JsonResult> OnGetExistsUsername(string username)
    {
        var existsUserDb = await _context.Users.AnyAsync(u => u.Username.ToLower().Trim() == username.ToLower().Trim());
        var existsUser = existsUserDb ? true : false;
        return new JsonResult(new { existsUser });
    }

}
