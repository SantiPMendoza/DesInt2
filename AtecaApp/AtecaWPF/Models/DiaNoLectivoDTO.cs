using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtecaWPF.Models
{
    public class DiaNoLectivoDTO : CreateDiaNoLectivoDTO
    {
        public int Id { get; set; }

    }

    public class CreateDiaNoLectivoDTO
    {

        public DateOnly Fecha { get; set; }
    }
}
