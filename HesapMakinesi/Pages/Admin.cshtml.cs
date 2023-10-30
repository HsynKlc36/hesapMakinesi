using HesapMakinesi.Context;
using HesapMakinesi.Models;
using HesapMakinesi.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HesapMakinesi.Pages
{
    [Authorize(Policy = "RequireAdminRole")]

    public class AdminModel : PageModel
    {
        private readonly MyDbContext _db;

        public List<AdminMathListVM> AdminMathListVM { get; set; }

        public AdminModel(MyDbContext myDbContext)
        {
            _db = myDbContext;
        }


        public void OnGet(Guid id)
        {
            LoadViewData();

        }
        public IActionResult OnPostDeletedAsync([FromBody] Guid id)
        {

            var math=_db.Mathematics.FirstOrDefault(x => x.Id == id);
            if (math != null)
            {
                _db.Mathematics.Remove(math);
                _db.SaveChanges();
                return new JsonResult(new { isSuccess = true, message = "Silme i�lemi ba�ar�yla ger�ekle�tirildi." });
            }
            else
            {
                return new JsonResult(new { isSuccess = false, message = "Silme i�lemi ba�ar�s�z oldu. Matematik i�lemi bulunamad�." });
            }
            
           
        }

        private void LoadViewData()
        {
            var mathListWithUser=_db.Mathematics
                .Include(x => x.User) // Mathematics s�n�f�ndaki User ili�kisini Include ile �ekiyoruz
                .ToList();
            AdminMathListVM = mathListWithUser.OrderByDescending(x=>x.CreatedDate).Select(x => new AdminMathListVM//mattaki i�lem s�ras�na g�re b�y�kten k����e s�raland�!
            {
                MathId = x.Id,
                Sayi1 = x.Sayi1,
                Sayi2 = x.Sayi2,
                Sonuc = x.Sonuc,
                UserName = x.User.UserName,
            }).ToList();
            
        }
    }
}
