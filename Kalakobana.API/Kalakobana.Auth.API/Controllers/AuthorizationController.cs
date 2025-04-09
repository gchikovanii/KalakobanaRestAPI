using Google.Apis.Auth;
using Kalakobana.Auth.Application.Commands;
using Kalakobana.Auth.Application.Models;
using Kalakobana.Auth.Application.Services.JWT;
using Kalakobana.Auth.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kalakobana.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _tokenGenerator;
        public AuthorizationController(IMediator mediator, UserManager<User> userManager, IJwtTokenGenerator tokenGenerator)
        {
            _mediator = mediator;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Failed)
                return Unauthorized(new { error = result.Error });

            return Ok(result.ResultValue);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Failed)
                return Unauthorized(new { error = result.Error });

            return Ok(result.ResultValue);
        }

        [HttpPost("external-login/google")]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            //Token Validation
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            }
            catch
            {
                return BadRequest("Invalid Google token");
            }

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user is null)
            {
                user = new User
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    return BadRequest("Failed to create user");
                await _userManager.AddToRoleAsync(user, "User");
            }

            var jwtToken = _tokenGenerator.GenerateToken(user, "User");
            return Ok(new { token = jwtToken, role = "User", email = payload.Email });
        }

        [HttpPost("external-login/facebook")]
        public async Task<IActionResult> FacebookLogin([FromBody] ExternalLoginDto dto)
        {
            var accessToken = dto.IdToken;

            // Validate token and get user info from Facebook
            using var httpClient = new HttpClient();
            var fbUrl = $"https://graph.facebook.com/me?access_token={accessToken}&fields=id,name,email";

            var response = await httpClient.GetAsync(fbUrl);
            if (!response.IsSuccessStatusCode)
                return BadRequest("Invalid Facebook token");

            var fbData = await response.Content.ReadFromJsonAsync<FacebookUserDto>();
            if (fbData is null || string.IsNullOrWhiteSpace(fbData.Email))
                return BadRequest("Unable to retrieve Facebook user");


            var user = await _userManager.FindByEmailAsync(fbData.Email);
            if (user is null)
            {
                Random random = new Random();
                user = new User
                {
                    UserName = fbData.Name + fbData.Email.Substring(0, 2) + random.Next(1, 1000),
                    Email = fbData.Email,
                    EmailConfirmed = true
                };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    return BadRequest("User creation failed");

                await _userManager.AddToRoleAsync(user, "User");
            }
            var jwt = _tokenGenerator.GenerateToken(user, "User");
            return Ok(new { token = jwt });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? Ok("Email confirmed!") : BadRequest("Invalid token.");
        }
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Succeeded)
                return Ok("Password reset email sent.");
            return BadRequest(result.Error);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Succeeded)
                return Ok("Password has been reset.");

            return BadRequest(result.Error);
        }


    }
}
