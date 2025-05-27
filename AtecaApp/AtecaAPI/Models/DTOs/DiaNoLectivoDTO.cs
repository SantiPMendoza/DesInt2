namespace AtecaAPI.Models.DTOs
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
