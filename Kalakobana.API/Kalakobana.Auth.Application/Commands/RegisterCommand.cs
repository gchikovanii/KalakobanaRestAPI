using Kalakobana.SharedKernel.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Kalakobana.Auth.Application.Commands
{
    public record RegisterCommand(RegisterRequest RegistrationData) : IRequest<Result<RegisterResult>>;
    public record RegisterResult(string Token, string Email, string Role);
    public class RegisterRequest
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "FullName is required")]
        public string UserName { get; set; }
    }

}
