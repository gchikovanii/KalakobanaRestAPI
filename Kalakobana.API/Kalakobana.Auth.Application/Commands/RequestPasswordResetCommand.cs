using Kalakobana.SharedKernel.Results;
using MediatR;

namespace Kalakobana.Auth.Application.Commands
{
    public class RequestPasswordResetCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }
}
