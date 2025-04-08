using Kalakobana.SharedKernel.Results;
using MediatR;

namespace Kalakobana.Auth.Application.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResult>>;
    public record LoginResult(string Token, string Email, string Role);
}
