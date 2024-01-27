using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventory.Data;
using RPInventory.Models;
using X.PagedList;

namespace RPInventory.Pages.Users;

public class IndexModel : PageModel
{
    private readonly InventoryContext _context;
    private readonly IConfiguration _configuration;

    public IndexModel(InventoryContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public IPagedList<User> Users { get; set; }
    [BindProperty(SupportsGet = true)]
    public int? PageNumber { get; set; }
    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; }
    public int TotalRecords { get; set; }

    public async Task OnGetAsync()
    {
        var recordsPerPage = _configuration.GetValue("RecordsPerPage", 3);

        var consulta = _context.Users
                                .Include(u => u.Profile)
                                .AsNoTracking()
                                .Select(u => u);

        if (!String.IsNullOrEmpty(SearchTerm))
        {
            consulta = consulta.Where(u => u.Name.Contains(SearchTerm) || u.Username.Contains(SearchTerm));
        }

        TotalRecords = consulta.Count();
        var pageNumber = PageNumber ?? 1;

        Users = await consulta.ToPagedListAsync(pageNumber, recordsPerPage);
    }

}
