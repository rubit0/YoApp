using AutoMapper;
using YoApp.Core.Models;
using YoApp.Dtos.Account;
using YoApp.Dtos.Users;

namespace YoApp.Identity.Core
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
