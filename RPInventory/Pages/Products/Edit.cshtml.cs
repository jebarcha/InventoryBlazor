using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Helpers;
using RPInventory.Data;
using RPInventory.Models;
using RPInventory.ViewModels;

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
        public ProductCreationEditionViewModel Product { get; set; }
        public SelectList Brands { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                _serviceNotification.Warning($"Product Id Should not be null");
                return NotFound();
            }

            var productDb = await _context.Products
                                        //.Include(p => p.Marca)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (productDb == null)
            {
                _serviceNotification.Warning($"Product Id Not Found");
                return NotFound();
            }

            Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");

            Product = new ProductCreationEditionViewModel()
            {
                Id = productDb.Id,
                Price = productDb.Price,
                Description = productDb.Description,
                Status = productDb.Status,
                BrandId = productDb.BrandId,
                Name = productDb.Name
            };

            if (!String.IsNullOrEmpty(productDb.Image))
            {
                Product.Image = await Utilerias.ConvertirImagenABytes(productDb.Image);
            }

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

            var productDb = await _context.Products.FindAsync(Product.Id);

            productDb.Price = Product.Price;
            productDb.Description = Product.Description;
            productDb.Status = Product.Status;
            productDb.BrandId = Product.BrandId;
            productDb.Name = Product.Name;

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                productDb.Image = await Utilerias.LeerImagen(file);
            }

            //_context.Attach(productDb).State = EntityState.Modified;

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
