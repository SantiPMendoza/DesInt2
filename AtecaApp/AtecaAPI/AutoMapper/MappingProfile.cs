using AutoMapper;
using AtecaAPI.Models.Entity;
using AtecaAPI.Models.DTOs;
using AtecaAPI.Models.DTOs.ReservaDTOs;


namespace AtecaAPI.AutoMapper
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Administrador, AdministradorDTO>().ReverseMap();
            CreateMap<Administrador, CreateAdministradorDTO>().ReverseMap();

            // Profesor
            CreateMap<Profesor, ProfesorDTO>();
            CreateMap<CreateProfesorDTO, Profesor>();

            
            // Reserva
            CreateMap<Reserva, ReservaDTO>();
            CreateMap<CreateReservaDTO, Reserva>();

            /**
            // GrupoClase
            CreateMap<GrupoClase, GrupoClaseDTO>();
            CreateMap<CreateGrupoClaseDTO, GrupoClase>();*/
        }
    }

}
