using AutoMapper;
using HesapMakinesi.Context;
using HesapMakinesi.Models;
using HesapMakinesi.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HesapMakinesi.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly MyDbContext _myDbContext;
        private readonly IMapper _mapper;

        [BindProperty]
        public RegisterVM Register { get; set; }
        
        
        public RegisterModel(MyDbContext myDbContext,IMapper mapper)
        {
            _myDbContext = myDbContext;
            _mapper = mapper;

        }
        //[NonHandler]
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            
          
                var currentUser=CheckUser(Register.Email);
                if (currentUser is not null) {
                    TempData["Message"] = "Bu Email de zaten bir kullanýcý var";
                    return Page();
                }
                var user =  _mapper.Map<RegisterVM, User>(Register);
                user.Password = await HashPasswordAsync(user.Password);
                _myDbContext.Users.Add(user);
                _myDbContext.SaveChanges();
                return RedirectToPage("/Auth/Login");
           
        }
        private User? CheckUser(string email) { 
           var user =_myDbContext.Users.Where(x=>x.Email==email).FirstOrDefault();
            return user;
        }
        private async Task<string> HashPasswordAsync(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
            return hashedPassword;
        }
    }
}
