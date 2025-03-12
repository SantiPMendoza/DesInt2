using AutoMapper;
using RepasoAPI.Models.DTOs;
using RepasoAPI.Models.DTOs.UserDTO;
using RepasoAPI.Models.Entity;


namespace RepasoAPI.AutoMapper
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
