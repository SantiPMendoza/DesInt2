
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtecaWPF.Models
{
    public class ReservaDTO
    {
        public int Id { get; set; }

        public ProfesorDTO Profesor { get; set; } = null!;
        public string ProfesorNombre => $"{Profesor.Nombre}";
        public GrupoClaseDTO GrupoClase { get; set; } = null!;
        public string GrupoClaseNombre => $"{GrupoClase.Nombre}";
        public string Estado { get; set; } = "Pendiente";
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaResolucion { get; set; }
    }
}
