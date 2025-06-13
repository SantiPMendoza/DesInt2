using AutoMapper;
using ExamenAPI.Models.Entity;
using ExamenAPI.Models.DTOs;


namespace ExamenAPI.AutoMapper
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            
            CreateMap<Administrador, AdministradorDTO>().ReverseMap();
            CreateMap<Administrador, CreateAdministradorDTO>().ReverseMap();

            // Student
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Student, CreateStudentDTO>().ReverseMap();

            // Course
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<Course, CreateCourseDTO>().ReverseMap();

            // Teacher
            CreateMap<Teacher, TeacherDTO>().ReverseMap();
            CreateMap<Teacher, CreateTeacherDTO>().ReverseMap();
            /**
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
            CreateMap<CreateGrupoClaseDTO, GrupoClase>();*/
        }
    }

}
