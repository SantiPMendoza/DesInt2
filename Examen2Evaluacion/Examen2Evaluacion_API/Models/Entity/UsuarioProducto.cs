namespace Examen2Evaluacion_API.Models.Entity
{
    public class UsuarioProducto
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

    }
}
