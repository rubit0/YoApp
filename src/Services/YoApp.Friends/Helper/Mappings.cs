using AutoMapper;
using YoApp.Data.Models;
using YoApp.DataObjects.Users;

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
