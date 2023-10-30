using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HesapMakinesi.Models
{
     public class Mathematics
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Sayı 1 alanı boş bırakılamaz")]
        public double Sayi1 { get; set; }
        [Required(ErrorMessage = "Sayı 2 alanı boş bırakılamaz")]
        public double Sayi2 { get; set; }
        public double? Sonuc { get; set; }
        public DateTime? CreatedDate { get; set; }=DateTime.Now;
        public double Topla(double sayi1,double sayi2) => sayi1 + sayi2;
        public double Cıkar(double sayi1, double sayi2) => sayi1 - sayi2;
        public double Carp(double sayi1, double sayi2) => sayi1 * sayi2;
        public double Bol(double sayi1, double sayi2) => sayi1 / sayi2;
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
