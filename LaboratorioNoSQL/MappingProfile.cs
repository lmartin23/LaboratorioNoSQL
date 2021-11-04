using AutoMapper;
using LaboratorioNoSQL.Dtos;
using LaboratorioNoSQL.Models;

namespace LaboratorioNoSQL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, Usuario>();
            CreateMap<Usuario, UserDto>();

            CreateMap<BaseUserDto, Usuario>();
            CreateMap<Usuario, BaseUserDto>();
        }
    }
}
