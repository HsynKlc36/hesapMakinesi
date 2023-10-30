using HesapMakinesi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HesapMakinesi.Models
{
    public class User 
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }
        //navigation property
        virtual public IEnumerable<Mathematics>? MathematicsList { get; set; }
    }
}
