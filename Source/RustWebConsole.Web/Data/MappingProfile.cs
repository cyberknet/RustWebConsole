using AutoMapper;
using RustWebConsole.Web.Data.DTOs;
using RustWebConsole.Web.Data.Entities;

namespace RustWebConsole.Web.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Server, ServerDto>().ReverseMap();
        }
    }
}