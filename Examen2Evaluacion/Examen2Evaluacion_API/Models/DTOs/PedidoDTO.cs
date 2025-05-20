namespace Examen2Evaluacion_API.Models.DTOs
{
    public class PedidoDTO : CreatePedidoDTO
    {
        public int Id { get; set; }
        //public Usuario Usuario { get; set; } => en caso de que se necesite devolver la info del usuario
    }

    public class CreatePedidoDTO
    {
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public List<int> Productos { get; set; } = [];
    }
}
