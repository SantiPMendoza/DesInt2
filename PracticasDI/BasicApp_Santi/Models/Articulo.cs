using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicApp_Santi.Models
{
    public class Articulo
    {
        public int Id { get; set; }

        public SportType? Deporte { get; set; } = SportType.otro;
        public ArticleType? TipoArticulo { get; set; } = ArticleType.articulo;

        public string? Nombre { get; set; } = string.Empty;
        public decimal? Precio { get; set; } = decimal.Zero;

        public Articulo() { }

        public enum SportType
        {
            futbol,
            baloncesto,
            tenis,
            esqui,
            boxeo,
            otro
        }

        public enum ArticleType
        {

            accesorio,
            ropa,
            zapatillas,
            articulo
        }


    }
}
