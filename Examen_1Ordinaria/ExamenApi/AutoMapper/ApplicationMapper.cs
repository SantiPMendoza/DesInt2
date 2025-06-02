using AutoMapper;
using ExamenApi.Models;
using ExamenApi.Models.DTOs.JuegoDTO;
using ExamenApi.Models.DTOs.UserDTO;

namespace ExamenApi.AutoMapper
{
    public class ApplicationMapper : Profile
    {
        
        public ApplicationMapper()
        {
            
            CreateMap<Juego, JuegoDTO>().ReverseMap();
            CreateMap<Juego, CreateJuegoDTO>().ReverseMap();


            CreateMap<AppUser, UserDTO>().ReverseMap();
        }
    }
}
