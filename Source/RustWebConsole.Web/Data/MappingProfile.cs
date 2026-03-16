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
            CreateMap<Player, PlayerDto>().ReverseMap();
            CreateMap<ConsoleMessage, ConsoleMessageDto>().ReverseMap();
            CreateMap<ServerStatistics, ServerStatisticsDto>().ReverseMap();
            CreateMap<PlayerInventory, PlayerInventoryDto>().ReverseMap();
            CreateMap<RconRequest, RconRequestDto>().ReverseMap();
            CreateMap<RconResponse, RconResponseDto>().ReverseMap();
        }
    }
}