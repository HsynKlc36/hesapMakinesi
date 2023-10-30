using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HesapMakinesi.VM
{
    public class LoginVM
    {
         
        [DisplayName("Email")]
        public string Email { get; set; }
        
        [DisplayName("Şifre")]
        public string Password { get; set; }
    }
}
