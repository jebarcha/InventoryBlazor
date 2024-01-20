using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Products
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
        public Product Product { get; set; }
        public SelectList Brands { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                _serviceNotification.Warning($"Product Id Should not be null");
                return NotFound();
            }

            Product = await _context.Products
                                        //.Include(p => p.Marca)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                _serviceNotification.Warning($"Product Id Not Found");
                return NotFound();
            }

            Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _serviceNotification.Error($"Fix the problems to edit the product {Product.Name}");
                Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
                return Page();
            }

            var existeProductoBd = _context.Products.Any(u => u.Name.ToLower().Trim() == Product.Name.ToLower().Trim()
                                                       && u.Id != Product.Id);

            if (existeProductoBd)
            {
                Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
                _serviceNotification.Warning($"Product Name {Product.Name} already exists");
                return Page();
            }


            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _serviceNotification.Success($"Product updated successfully {Product.Name}");
            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

    }
}
