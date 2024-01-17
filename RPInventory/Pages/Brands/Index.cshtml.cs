using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;

namespace RPInventory.Pages.Brands
{
    public class IndexModel : PageModel
    {
        private readonly RPInventory.Data.InventoryContext _context;

        public IndexModel(RPInventory.Data.InventoryContext context)
        {
            _context = context;
        }

        public IList<Brand> Brand { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Brand = await _context.Brands.ToListAsync();
        }
    }
}
