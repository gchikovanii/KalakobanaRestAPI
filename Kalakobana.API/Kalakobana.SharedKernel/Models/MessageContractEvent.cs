namespace Kalakobana.SharedKernel.Models
{
    public class MessageContractEvent
    {
        public string SendTo { get; set; }
        public string Subject { get; set; }
        public string Link { get; set; }
        public string ActionType { get; set; }
    }
}
