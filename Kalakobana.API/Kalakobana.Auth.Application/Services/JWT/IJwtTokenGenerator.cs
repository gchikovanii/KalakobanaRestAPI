using Kalakobana.Auth.Domain;

namespace Kalakobana.Auth.Application.Services.JWT
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, string role);
    }
}
