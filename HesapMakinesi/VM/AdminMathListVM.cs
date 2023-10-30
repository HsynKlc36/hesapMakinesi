using HesapMakinesi.Models;
using System.ComponentModel.DataAnnotations;

namespace HesapMakinesi.VM
{
    public class AdminMathListVM
    {
        public Guid MathId { get; set; }       
        public double Sayi1 { get; set; }
        public double Sayi2 { get; set; }
        public double? Sonuc { get; set; }
        public string UserName { get; set; } 
    }
}
