using System.ComponentModel.DataAnnotations;

namespace Examen2Evaluacion_API.Models.DTOs
{
    public class ProductoDTO : CreateProductoDTO
    {
        public int Id { get; set; }
    }

    public class CreateProductoDTO
    {
        [Required(ErrorMessage = "Field required: Nombre")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Field required: Precio")]
        public double Precio { get; set; }
    }
}
