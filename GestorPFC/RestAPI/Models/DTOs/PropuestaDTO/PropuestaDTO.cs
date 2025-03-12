namespace RestAPI.Models.DTOs.PropuestaDTO
{
    public class PropuestaDto : CreatePropuestaDTO
    {
        public int Id { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaGestion { get; set; }

        public string Estado { get; set; } = null!;
    }
}
