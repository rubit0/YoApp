using AutoMapper;
using YoApp.Core.Dtos.Account;
using YoApp.Core.Dtos.Users;
using YoApp.Core.Models;

namespace YoApp.Identity.Helper
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<CreatedAccountDto, ApplicationUser>()
                .ForMember(a => a.UserName, o => o.MapFrom(u => u.PhoneNumber));

            CreateMap<ApplicationUser, CreatedAccountDto>()
                .ForMember(u => u.PhoneNumber, o => o.MapFrom(a => a.UserName));

            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
