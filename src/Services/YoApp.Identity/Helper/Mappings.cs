using AutoMapper;
using YoApp.Data.Models;
using YoApp.DataObjects.Account;
using YoApp.DataObjects.Users;

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
