using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtecaWPF.Models
{
    public class ProfesorDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!; // Solo correos @iescomercio.com
        public string GoogleId { get; set; } = null!;

        //public ICollection<ReservaDTO> Reservas { get; set; } = [];
    }
}
