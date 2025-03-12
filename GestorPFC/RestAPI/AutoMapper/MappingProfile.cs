using AutoMapper;

using RestAPI.Models.DTOs.AlumnoDTO;
using RestAPI.Models.DTOs.CursoDTO;
using RestAPI.Models.DTOs.DepartamentoDTO;
using RestAPI.Models.DTOs.ProfesorDTO;
using RestAPI.Models.DTOs.PropuestaDTO;
using RestAPI.Models.DTOs.ProyectoDTO;
using RestAPI.Models.DTOs.UserDTO;
using RestAPI.Models.Entity;

namespace RestAPI.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeos para Alumno
            CreateMap<Alumno, AlumnoDto>();
            CreateMap<CreateAlumnoDTO, Alumno>();

            // Mapeos para Curso
            CreateMap<Curso, CursoDto>();
            CreateMap<CreateCursoDTO, Curso>();

            // Mapeos para Departamento
            CreateMap<Departamento, DepartamentoDto>();
            CreateMap<CreateDepartamentoDTO, Departamento>();

            // Mapeos para Profesor
            CreateMap<Profesor, ProfesorDto>();
            CreateMap<CreateProfesorDTO, Profesor>();

            // Mapeos para Propuesta
            CreateMap<Propuesta, PropuestaDto>();
            CreateMap<CreatePropuestaDTO, Propuesta>();

            // Mapeos para Proyecto
            CreateMap<Proyecto, ProyectoDto>();
            CreateMap<CreateProyectoDTO, Proyecto>();

            // Mapeos para User (AppUser)
            CreateMap<AppUser, UserDTO>();
        }
    }
}
