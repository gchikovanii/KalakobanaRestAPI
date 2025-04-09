namespace Kalakobana.Game.Domain.Models
{
    public class PlayerScoresResponseDTO
    {
        public Guid UserId { get; set; }
        public int FirstNamePoint { get; set; }
        public int LastNamePoint { get; set; }
        public int CountryPoint { get; set; }
        public int CityPoint { get; set; }
        public int PlantPoint { get; set; }
        public int AnimalPoint { get; set; }
        public int MoviePoint { get; set; }
        public int RiverPoint { get; set; }
    }
}
