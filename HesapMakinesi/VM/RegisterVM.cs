using HesapMakinesi.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HesapMakinesi.VM
{
    public class RegisterVM
    {
        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Şifre")]
        public string Password { get; set; }
        [DisplayName("Şifre Doğrulama")]
        public string RetypedPassword { get; set; }
        [BindNever]//bind ederken bu veriyi hidden tuttuğum için invalid gelmesini önler çünkü rolü nullable işaretlemediğim için burada , eğerki bu attribute ile işaretlemeseydim bind ederken invalid hatası alacaktı.Fluent validasyonu kullanırken test et
        public Role Role { get; set; } = Role.Guest;
        [BindNever]
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public DateTime? DeletedDate { get; set; }
        [BindNever]
        public bool IsActive { get; set; } = true;
    }
}
