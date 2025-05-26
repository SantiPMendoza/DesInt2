using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Examen2Evaluacion_API.Models.Entity
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]  
        public double Precio { get; set; }

        public ICollection<PedidoProducto> PedidoProductos { get; set; } = [];
        public ICollection<UsuarioProducto> UsuarioProductos { get; set; } = [];
    }

}
