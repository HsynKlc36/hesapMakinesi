using AutoMapper;
using HesapMakinesi.Models;
using HesapMakinesi.VM;

namespace HesapMakinesi.MapperUI.Profiles
{
    public class RegisterVMProfile: Profile
    {
        public RegisterVMProfile()
        {
            CreateMap<RegisterVM,User>().ReverseMap();
        }
    }
}
