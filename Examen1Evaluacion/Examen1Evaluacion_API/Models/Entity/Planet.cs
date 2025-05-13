namespace Examen1Evaluacion_API.Models.Entity
{
    public class Planet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public enum PlanetType
        {
            Terrestrial,
            GasGiant,
            IceGiant
        }
        public string Atmosphere { get; set; }
        public int Temperature { get; set; }
        public string ImageName { get; set; }
    }
}
