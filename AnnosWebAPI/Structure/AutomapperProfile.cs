using AnnosWebAPI.Data;
using AutoMapper;

namespace AnnosWebAPI.Structure
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Advertisment, AdvertismentVM>().ReverseMap();
        }
    }
}
