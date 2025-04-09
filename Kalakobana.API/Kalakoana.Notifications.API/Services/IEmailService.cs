using Kalakobana.SharedKernel.Models;

namespace Kalakoana.Notifications.API.Services
{
    public interface IEmailService
    {
        Task SendEmail(MessageContractEvent request, CancellationToken token);
    }
}
