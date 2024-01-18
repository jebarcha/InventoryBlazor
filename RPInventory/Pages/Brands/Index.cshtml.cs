using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;
using X.PagedList;

namespace RPInventory.Pages.Brands
{
    public class IndexModel : PageModel
    {
        private readonly InventoryContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(InventoryContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IPagedList<Brand> Brands { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public int? Page { get; set; }

        [BindProperty(SupportsGet = true)] 
        public string SearchTerm { get; set; }

        public int TotalRecords { get; set; }

        public async Task OnGetAsync()
        {
            var recordsPerPage = _configuration.GetValue("RecordsPerPage", 3);
            
            var result = _context.Brands.Select(u => u);

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                result = result.Where(u => u.Name.Contains(SearchTerm));
            }
            
            TotalRecords = result.Count();
            var pageNumber = Page ?? 1;

            Brands = await result.ToPagedListAsync(pageNumber, recordsPerPage);
        }
    }
}
