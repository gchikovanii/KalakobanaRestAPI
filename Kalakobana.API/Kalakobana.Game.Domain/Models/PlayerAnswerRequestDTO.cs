namespace Kalakobana.Game.Domain.Models
{
    public class PlayerAnswerRequestDTO
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Plant { get; set; }
        public string Animal { get; set; }
        public string Movie { get; set; }
        public string River { get; set; }
    }
}
