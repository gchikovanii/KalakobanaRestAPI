using Kalakobana.Auth.Domain;
using Kalakobana.SharedKernel.Models;
using Kalakobana.SharedKernel.Results;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace Kalakobana.Auth.Application.Commands
{
    public class RequestPasswordResetCommandHandler
    {
        private readonly UserManager<User> _userManager;
        private readonly IPublishEndpoint _publishEndpoint;

        public RequestPasswordResetCommandHandler(UserManager<User> userManager, IPublishEndpoint publishEndpoint)
        {
            _userManager = userManager;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Failure("No account with this email.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://yourfrontend.com/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            var message = new MessageContractEvent
            {
                SendTo = user.Email,
                Subject = "Reset Your Password",
                ActionType = MessageType.PasswordReset.ToString(),
                Link = resetLink
            };

            await _publishEndpoint.Publish(message, cancellationToken);
            return Result.Success();
        }
    }
}
