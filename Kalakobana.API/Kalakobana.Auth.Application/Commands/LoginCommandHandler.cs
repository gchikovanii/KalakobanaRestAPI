using Kalakobana.Auth.Application.Services.JWT;
using Kalakobana.Auth.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kalakobana.Auth.Application.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenGenerator _tokenGenerator;
        public LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) throw new UnauthorizedAccessException("Invalid Email");
            if (user.EmailConfirmed == false)
                throw new UnauthorizedAccessException("Email is not confirmed");
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) throw new UnauthorizedAccessException("Invalid Email or Password");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";
            var token = _tokenGenerator.GenerateToken(user, role);

            return new LoginResult(token, user.Email, role);
        }
    }
}
