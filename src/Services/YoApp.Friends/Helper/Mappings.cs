using AutoMapper;
using YoApp.Core.Dtos.Users;
using YoApp.Core.Models;

namespace YoApp.Identity.Helper
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
