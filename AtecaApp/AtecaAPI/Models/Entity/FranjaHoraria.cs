namespace AtecaAPI.Models.Entity
{
    public class FranjaHoraria
    {
        public int Id { get; set; }

        //public DayOfWeek DiaSemana { get; set; } // Lunes = 1, etc.
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }

        public bool Activo { get; set; } = true; // Permite habilitar/deshabilitar bloques fácilmente
    }

}
