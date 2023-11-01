using HesapMakinesi.Context;
using HesapMakinesi.Models;
using HesapMakinesi.VM;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HesapMakinesi.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly MyDbContext _myDbContext;

        public LoginModel(MyDbContext myDbContext)
        {
           
            _myDbContext = myDbContext;
        }

        [BindProperty]
        public LoginVM LoginVM { get; set; }
        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
           
                var currentUser = await _myDbContext.Users.FirstOrDefaultAsync(u => u.Email == LoginVM.Email,CancellationToken.None);
                
                if (currentUser is null)
                {
                    TempData["Message"] = "Böyle bir kullanýcý bulunamadý!";
                    return Page();
                }

                if (currentUser is not null&& VerifyPassword(LoginVM.Password,currentUser.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim("Id",currentUser.Id.ToString()),
                        new Claim(ClaimTypes.Name,currentUser.UserName),
                        new Claim(ClaimTypes.Email,LoginVM.Email),
                        new Claim(ClaimTypes.Role,currentUser.Role.ToString())
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var props = new AuthenticationProperties();
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity),props);
                    if (currentUser.Role==Models.Enums.Role.Admin) {
                        return RedirectToPage("/Admin");
                    }
                    return RedirectToPage("/Index");//customer için giriþ sayfasý
                }
                else
                {
                    TempData["Message"] = "Kullanýcýya ait þifre hatalý!";
                    return Page();
                }
           
            
        }
        
        //public async Task<IActionResult> OnPostLogoutAsync() {
        //    await HttpContext.SignOutAsync();
        //    HttpContext.Response.Cookies.Delete("HesapMakinesi");//cookie'yi tamamen tarayýcýdan siler!
        //    return RedirectToPage("/Index");
        //}
      
        private bool VerifyPassword(string password, string hashedPassword)
        {
            // Hashli þifreyi doðrulama
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return passwordMatch;
        }
    }
}
