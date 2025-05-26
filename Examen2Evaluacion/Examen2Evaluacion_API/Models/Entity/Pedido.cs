using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Examen2Evaluacion_API.Models.Entity
{
public class Pedido
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public ICollection<PedidoProducto> PedidoProductos { get; set; } = [];

}


}
