using Kalakobana.SharedKernel.Results;
using MediatR;

namespace Kalakobana.Auth.Application.Commands
{
    public class ResetPasswordCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
