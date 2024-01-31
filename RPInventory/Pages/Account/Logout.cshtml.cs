using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RPInventory.Pages.Account
{
    public class LogoutModel : PageModel
    {

        public LogoutModel()
        {
            
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            
            // Clean up cookies
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }

    }
}
