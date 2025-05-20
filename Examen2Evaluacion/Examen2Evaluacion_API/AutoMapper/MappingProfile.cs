using AutoMapper;
using Examen2Evaluacion_API.Models.DTOs;
using Examen2Evaluacion_API.Models.DTOs.UserDTO;
using Examen2Evaluacion_API.Models.Entity;


namespace Examen2Evaluacion_API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeos para Alumno
            CreateMap<Producto, ProductoDTO>();
            CreateMap<CreateProductoDTO, Producto>();

            // Mapeos para Curso
            CreateMap<Usuario, PedidoDTO>();
            CreateMap<CreatePedidoDTO, Usuario>();

            // Mapeos para Departamento
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<CreateUsuarioDTO, Usuario>();


            // Mapeos para User (AppUser)
            CreateMap<AppUser, UserDTO>();



        }
    }
}
