namespace AtecaAPI.Models.DTOs
{
    public class FranjaHorariaDTO : CreateFranjaHorariaDTO
    {
        public int Id { get; set; }

        public bool Activo { get; set; } = true;

    }

    public class CreateFranjaHorariaDTO
    {

        public DayOfWeek DiaSemana { get; set; } // Lunes = 1, etc.
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }
}
