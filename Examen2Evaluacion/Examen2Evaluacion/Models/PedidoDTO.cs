using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen2Evaluacion.Models
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public List<int> Productos { get; set; } = [];
    }

}
