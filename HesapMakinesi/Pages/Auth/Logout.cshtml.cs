using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HesapMakinesi.Pages.Auth
{
    [Authorize(Policy = "RequireAdminOrGuestRole")]
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Oturumu kapatma iþlemlerini burada gerçekleþtirin
            await HttpContext.SignOutAsync();
            HttpContext.Response.Cookies.Delete("HesapMakinesi");

            return RedirectToPage("/Auth/Login");
        }
    }
}
