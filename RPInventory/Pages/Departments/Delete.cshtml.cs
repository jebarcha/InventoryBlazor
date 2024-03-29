using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Departments;

public class DeleteModel : PageModel
{

    private readonly Data.InventoryContext _context;
    private readonly INotyfService _serviceNotification;

    public DeleteModel(InventoryContext context, INotyfService servicioNotificacion)
    {
        _context = context;
        _serviceNotification = servicioNotificacion;
    }

    [BindProperty]
    public Department Department { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotification.Warning($"Id should contain a value.");
            return NotFound();
        }

        Department = await _context.Departments.FirstOrDefaultAsync(m => m.Id == id);

        if (Department == null)
        {
            _serviceNotification.Warning($"Department not found with specified Id");
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

        Department = await _context.Departments.FindAsync(id);

        if (Department != null)
        {
            _context.Departments.Remove(Department);
            await _context.SaveChangesAsync();
        }
        _serviceNotification.Success($"Department {Department.Name} deleted");
        return RedirectToPage("./Index");
    }
}
