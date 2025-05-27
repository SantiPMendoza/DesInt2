using AutoMapper;
using AtecaAPI.Models.Entity;
using AtecaAPI.Models.DTOs;


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

            // FranjaHoraria
            CreateMap<FranjaHoraria, FranjaHorariaDTO>();
            CreateMap<CreateFranjaHorariaDTO, FranjaHoraria>();

            // DiaNoLectivo
            CreateMap<DiaNoLectivo, DiaNoLectivoDTO>();
            CreateMap<CreateDiaNoLectivoDTO, DiaNoLectivo>();

            // GrupoClase
            CreateMap<GrupoClase, GrupoClaseDTO>();
            CreateMap<CreateGrupoClaseDTO, GrupoClase>();
        }
    }

}
