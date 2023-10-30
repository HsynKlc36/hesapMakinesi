using FluentValidation;
using HesapMakinesi.VM;
using System.Text.RegularExpressions;

namespace HesapMakinesi.FluentValidatiors.AuthValidators
{
    public class AuthRegisterVMValidator : AbstractValidator<RegisterVM>
    {
        public AuthRegisterVMValidator()
        {
            RuleFor(x => x.UserName)
            .NotEmpty()
            .WithName("Kullanıcı Adı")
            .WithMessage("{PropertyName} alanı boş bırakılamaz.")
            .Length(2, 15)
            .WithName("Kullanıcı Adı")
            .WithMessage("{PropertyName}  {MinLength} - {MaxLength} aralığında olmak zorundadır.")
            .Matches(new Regex(@"^[a-zA-ZıüöçşğİÜÖÇŞĞ]+$"))
            .WithMessage("lütfen sadece harf giriniz");

            RuleFor(x => x.Password)
          .NotEmpty()
          .WithName("Şifre")
          .WithMessage("{PropertyName} alanı boş bırakılamaz.")
          .Matches(new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{4,}$"))
          .WithMessage("Şifre en az 4 karakter uzunluğunda olmalı ve en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.");

            RuleFor(x => x.RetypedPassword)
                .NotEmpty()
                .WithName("Şifre Doğrulama")
                .WithMessage("Şifre doğrulama alanı boş olamaz.")
            .Matches(new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{4,}$"))
            .WithMessage("Şifre doğrulama en az 4 karakter uzunluğunda olmalı ve en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.")
            .Equal(x => x.Password).WithMessage("Şifreler eşleşmiyor.");

            RuleFor(x => x.Email)
                .NotEmpty().WithName("Email")
               .WithMessage("{PropertyName} alanı boş bırakılamaz.")
               .Matches(new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            .WithMessage("{PropertyName} formatında giriş yapınız.");
        }
    }
}
