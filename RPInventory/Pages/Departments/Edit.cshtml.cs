using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Departments;

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
    public Department Department { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotification.Warning($"Department Id should not be null.");
            return NotFound();
        }

        Department = await _context.Departments.FirstOrDefaultAsync(m => m.Id == id);

        if (Department == null)
        {
            _serviceNotification.Warning($"Department Not Found.");
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
            _serviceNotification.Error($"There's an error. Please fix the problems to create the department {Department.Name}");
            return Page();
        }

        var existeDepartamentoBd = _context.Departments.Any(u => u.Name.ToLower().Trim() == Department.Name.ToLower().Trim()
                                                    && u.Id != Department.Id);
        if (existeDepartamentoBd)
        {
            _serviceNotification.Warning($"Department alredy exists with name {Department.Name}");
            return Page();
        }


        _context.Attach(Department).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DepartamentoExists(Department.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        _serviceNotification.Success($"Department updated successfully {Department.Name}");
        return RedirectToPage("./Index");
    }

    private bool DepartamentoExists(int id)
    {
        return _context.Departments.Any(e => e.Id == id);
    }
}

