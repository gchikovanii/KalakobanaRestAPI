using Kalakobana.Auth.Application.Services.JWT;
using Kalakobana.Auth.Domain.Constants;
using Kalakobana.Auth.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Kalakobana.SharedKernel.Results;

namespace Kalakobana.Auth.Application.Commands
{
  public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResult>>
  {
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public RegisterCommandHandler(UserManager<User> userManager, IJwtTokenGenerator tokenGenerator)
    {
      _userManager = userManager;
      _tokenGenerator = tokenGenerator;
    }
    public async Task<Result<RegisterResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
      var existingUser = await _userManager.FindByEmailAsync(request.RegistrationData.Email);
      var userName = await _userManager.FindByNameAsync(request.RegistrationData.UserName);
      if (existingUser is not null)
        return Result<RegisterResult>.Failure("Email is already taken.");
      if (userName is not null)
        return Result<RegisterResult>.Failure($"User Name '{request.RegistrationData.UserName}' is already taken.");
      var newUser = new User
      {
        UserName = request.RegistrationData.UserName,
        Email = request.RegistrationData.Email,
        EmailConfirmed = false
      };

      var result = await _userManager.CreateAsync(newUser, request.RegistrationData.Password);
      if (!result.Succeeded)
      {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Result<RegisterResult>.Failure($"Registration failed: {errors}");
      }
      var role = Roles.User.ToString();
      await _userManager.AddToRoleAsync(newUser, role);

      var token = _tokenGenerator.GenerateToken(newUser, role);
      if (token is not null)
      {
        //Send Email To Confirm account here
      }
      return Result<RegisterResult>.Success(new RegisterResult(token, newUser.Email, role));

    }
  }
}
