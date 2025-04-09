namespace Kalakobana.Game.Domain.Models
{
    public class AnswerResponseDTO
    {
        public Guid RoomId { get; set; }
        public List<PlayerScoresResponseDTO> Answers { get; set; }
    }
}
