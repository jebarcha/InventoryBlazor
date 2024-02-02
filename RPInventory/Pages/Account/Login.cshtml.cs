using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.Helpers;
using RPInventarios.ViewModels;
using RPInventory.Data;
using RPInventory.Models;
using System.Security.Claims;

namespace RPInventory.Pages.Account;

public class LoginModel : PageModel
{
    private readonly InventoryContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly INotyfService _serviceNotification;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(InventoryContext context, IPasswordHasher<User> passwordHasher,
        INotyfService serviceNotification, ILogger<LoginModel> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _serviceNotification = serviceNotification;
        _logger = logger;
    }

    [BindProperty]
    public LoginViewModel LoginVM { get; set; }
    public string ReturnUrl { get; set; }

    public async Task OnGetAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var userDb = _context.Users
                            .Include(u => u.Profile)
                            .FirstOrDefault(u => u.Username.ToLower().Trim() == LoginVM.Username.ToLower().Trim());

            if (userDb == null)
            {
                _serviceNotification.Warning("User Name does not exist.");
                return Page();
            }

            var result = _passwordHasher.VerifyHashedPassword(userDb, userDb.Password, LoginVM.Password);

            if (result == PasswordVerificationResult.Success)
            {
                //Password is correct
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userDb.Username),
                    new Claim("FullName", userDb.Name + " " + userDb.Lastname),
                    new Claim(ClaimTypes.Role,userDb.Profile.Name)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = LoginVM.RememberMe
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation($"User {userDb.Username} has access the system. {DateTime.UtcNow}");

                return LocalRedirect(Url.GetLocalUrl(returnUrl));

            }
            else
            {
                _serviceNotification.Warning("Password is incorrect");
                return Page();
            }
        }
        return Page();
    }
}
