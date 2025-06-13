using Examen2Evaluacion_API.Models.Entity;

namespace Examen2Evaluacion_API.Models.DTOs
{
    public class PedidoDTO : CreatePedidoDTO
    {
        public int Id { get; set; }
        public UsuarioDTO Usuario { get; set; } = null!;
        public List<ProductoDTO> Productos { get; set; } = [];

    }

    public class CreatePedidoDTO
    {
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public List<int> ProductosId { get; set; } = [];
    }
}
