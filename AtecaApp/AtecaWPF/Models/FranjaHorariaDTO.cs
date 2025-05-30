using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtecaWPF.Models
{
    public class FranjaHorariaDTO : CreateFranjaHorariaDTO
    {
        public int Id { get; set; }

        public bool Activo { get; set; } = true;

    }

    public class CreateFranjaHorariaDTO
    {

        //public DayOfWeek DiaSemana { get; set; } // Lunes = 1, etc.
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }
}
