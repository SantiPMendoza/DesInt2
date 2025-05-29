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


    public DateOnly Fecha { get; set; } 
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }

    // Propiedad para mostrar la franja horaria como string
    public string DiaSemanaString => Fecha.DayOfWeek.ToString();
    public string FranjaHoraria => $"{HoraInicio:HH:mm} - {HoraFin:HH:mm}";
}
