using AutoMapper;
using YoApp.Backend.Models;
using YoApp.DataObjects.Users;

namespace YoApp.Backend.Helper
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<UsersDto, ApplicationUser>()
                .ForMember(a => a.UserName, o => o.MapFrom(u => u.PhoneNumber));
        }
    }
}
