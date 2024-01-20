using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;
using X.PagedList;

namespace RPInventory.Pages.Departments;

public class IndexModel : PageModel
{
    private readonly InventoryContext _context;
    private readonly IConfiguration _configuration;

    public IndexModel(InventoryContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public IPagedList<Department> Departments { get; set; }
    [BindProperty(SupportsGet = true)]
    public int? PageNumber { get; set; }
    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; }
    public int TotalRecords { get; set; }

    public async Task OnGetAsync()
    {
        var recordsPerPage = _configuration.GetValue("RecordsPerPage", 3);

        var result = _context.Departments
                               .AsNoTracking()
                               .Select(u => u);

        if (!string.IsNullOrEmpty(SearchTerm))
        {
            result = result.Where(u => u.Name.Contains(SearchTerm));
        }

        TotalRecords = result.Count();
        var pageNumber = PageNumber ?? 1;

        Departments = await result.ToPagedListAsync(pageNumber, recordsPerPage);
    }
}
