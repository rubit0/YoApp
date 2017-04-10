using AutoMapper;
using YoApp.Core.Models;
using YoApp.Dtos.Users;

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
