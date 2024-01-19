using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Departments;

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
    public Department Department { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _serviceNotification.Error($"There's an error. Please fix the problems to create the department {Department.Name}");
            return Page();
        }

        var existsDepartmentBd = _context.Departments.Any(u => u.Name.ToLower().Trim() == Department.Name.ToLower().Trim());
        if (existsDepartmentBd)
        {
            _serviceNotification.Warning($"Department Name {Department.Name} already exists");
            return Page();
        }

        _context.Departments.Add(Department);
        await _context.SaveChangesAsync();
        _serviceNotification.Success($"Department created successfully {Department.Name}");
        return RedirectToPage("./Index");
    }
}
