namespace Kalakobana.Game.Domain.Models
{
    public class AnswerRequestDTO
    {
        public Guid RoomId { get; set; }
        public List<PlayerAnswerRequestDTO> Answers { get; set; }
    }
}
