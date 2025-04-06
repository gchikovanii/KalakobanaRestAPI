using MediatR;

namespace Kalakobana.Auth.Application.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;
    public record LoginResult(string Token, string Email, string Role);
}
