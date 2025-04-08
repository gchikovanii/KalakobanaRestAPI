using Kalakobana.Auth.Application.Services.JWT;
using Kalakobana.Auth.Domain;
using Kalakobana.SharedKernel.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kalakobana.Auth.Application.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand,Result<LoginResult>>
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

        public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) throw new UnauthorizedAccessException("Invalid Email");
            if (user.EmailConfirmed == false)
                return Result<LoginResult>.Failure("Email is not confirmed");
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) return Result<LoginResult>.Failure("Invalid Email or Password");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";
            var token = _tokenGenerator.GenerateToken(user, role);

            return Result<LoginResult>.Success(new LoginResult(token, user.Email, role)); ;
        }
    }
}
