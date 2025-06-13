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
            // Producto
            CreateMap<Producto, ProductoDTO>();
            CreateMap<CreateProductoDTO, Producto>();

            // Usuario
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<CreateUsuarioDTO, Usuario>();

            // Pedido
            CreateMap<Pedido, PedidoDTO>();
            CreateMap<CreatePedidoDTO, Pedido>();

            CreateMap<Pedido, PedidoDTO>()
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.PedidoProductos.Select(pp => pp.Producto)))
                .ForMember(dest => dest.ProductosId, opt => opt.MapFrom(src => src.PedidoProductos.Select(pp => pp.ProductoId)))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario));


            CreateMap<CreatePedidoDTO, Pedido>()
                .ForMember(dest => dest.PedidoProductos, opt => opt.Ignore()); // Lo asignarás manualmente


            /**
            CreateMap<Pedido, PedidoDTO>()
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.Productos.Select(p => p.Id)));

            CreateMap<CreatePedidoDTO, Pedido>()
                .ForMember(dest => dest.Productos, opt => opt.Ignore()); // Se asignan manualmente

            */
            /** Para el posible mapeo de Usuario
             * 
                        CreateMap<Pedido, PedidoDTO>()
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.Productos.Select(p => p.Id)));

            CreateMap<CreatePedidoDTO, Pedido>()
                .ForMember(dest => dest.Productos, opt => opt.Ignore());
            
             */

            // AppUser
            CreateMap<AppUser, UserDTO>();
        }
    }
}
