using Kalakoana.Notifications.API.Services;
using Kalakobana.SharedKernel.Models;
using MassTransit;

namespace Kalakoana.Notifications.API.Brokers
{
    public class EmailConsumer : IConsumer<MessageContractEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailConsumer> _logger;

        public EmailConsumer(IEmailService emailService, ILogger<EmailConsumer> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<MessageContractEvent> context)
        {
            //Get Message
            var message = context.Message;
            //Send Message
            var contract = new MessageContractEvent
            {
                SendTo = message.SendTo,
                Subject = message.Subject,
                Link = message.Link,
                ActionType = message.ActionType
            };
            try
            {
                await _emailService.SendEmail(contract, context.CancellationToken);
                _logger.LogInformation("Email sent successfully to {EmailAddress}", contract.SendTo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending email to {EmailAddress}", contract.SendTo);
                throw;
            }

        }
    }
}
