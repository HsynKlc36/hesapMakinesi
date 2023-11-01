using HesapMakinesi.Context;
using HesapMakinesi.Models;
using HesapMakinesi.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Security.Claims;

namespace HesapMakinesi.Pages
{
    [Authorize(Policy = "RequireAdminOrGuestRole")]
 
    public class IndexModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        readonly ILogger<IndexModel> _logger;

        public IndexModel(MyDbContext dbContext, ILogger<IndexModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public string? UserId { get; set; }
        public Mathematics? FilterMath { get; set; } 

        [BindProperty]
        public Mathematics Mathematics { get; set; }

        public void OnGet(int? data, double sayi1, double sayi2)
        {
            try {
                GetUserId();
                LoadMathListViewData();


                if (data is not null)
                {
                    double sonuc;
                    Mathematics = new Mathematics();
                    if (data == (int)Operation.plus)
                    {
                        sonuc = Mathematics.Topla(sayi1, sayi2);
                    }
                    else
                    {
                        sonuc = Mathematics.Cıkar(sayi1, sayi2);
                    }
                    Mathematics.Sonuc = sonuc;
                    Mathematics.Sayi1 = sayi1;
                    Mathematics.Sayi2 = sayi2;
                    Mathematics.UserId = Guid.Parse(UserId);
                    _dbContext.Mathematics.Add(Mathematics);
                    _dbContext.SaveChanges();
                    _logger.LogInformation("Mathematics added");
                }
            }catch (Exception )
            {
                _logger.LogError("Mathematics Added Failed");
            }
            



        }
        public IActionResult OnPost(int? data)
        {
            GetUserId();
            double sonuc;
           
            if (data == (int)Operation.times)
            {
                sonuc = Mathematics.Carp(Mathematics.Sayi1, Mathematics.Sayi2);
            }
            else
            {
                if (Mathematics.Sayi2 == 0)
                {
                    throw new DivideByZeroException("sayı 0 ile bölünemez!");
                }
                sonuc = Mathematics.Bol(Mathematics.Sayi1, Mathematics.Sayi2);
            }
            Mathematics.Sonuc = sonuc;
            Mathematics.UserId = Guid.Parse(UserId);
            _dbContext.Mathematics.Add(Mathematics);
            _dbContext.SaveChanges();

            return RedirectToPage();

        }


        private void LoadMathListViewData()
        {
           
            //FilterMath = _dbContext.Mathematics.Where(x => x.UserId.ToString() == UserId).OrderByDescending(x => x.CreatedDate).AsNoTracking().FirstOrDefault();
            FilterMath = _dbContext.Mathematics.Where(x => x.UserId.ToString() == UserId).AsEnumerable().MaxBy(x => x.CreatedDate);//önbelleğe getirip sorguyu orada yapar!aksi halde maxby'ı veritabanında sorgularken hata ile karşılaşır
        }
        private void GetUserId()
        {
            UserId = HttpContext.User.FindFirstValue("Id");//istek atan kullanıcının cookie de var olan Id'sinialır
            
        }
    }
}