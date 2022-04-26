using DigitalProject.DB;
using DigitalProject.Models;
using DigitalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalProject.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{

    private readonly UserManager<Faculty> userManager;
    private readonly SignInManager<Faculty> signInManager;
    private readonly ITokenService tokenService;

    public AccountController(UserManager<Faculty> userManager,
        SignInManager<Faculty> signInManager,
        ITokenService tokenService) {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.tokenService = tokenService;
    }

    /// <summary>
    /// Logs a user in.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoggedInModel), 200)]
    [ProducesResponseType(typeof(ErrorMessageModel), 401)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login(LoginModel model) {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        var result = await signInManager.PasswordSignInAsync(model.OrediggerId, model.Password, false, lockoutOnFailure: false);
        if (result.Succeeded) {
            var user = await userManager.FindByIdAsync(model.OrediggerId);
            LoggedInModel loggedInModel = new LoggedInModel {
                Token = tokenService.BuildToken(user),
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrediggerId = user.UserName,
            };
            return Ok(loggedInModel);
        }
        return Unauthorized(new ErrorMessageModel("Invalid credentials"));
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoggedInModel), 200)]
    [ProducesResponseType(409)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register(RegistrationModel model) {
        if (!ModelState.IsValid) {
            return BadRequest();
        }

        var user = new Faculty{ 
            FirstName = model.FistName,
            MiddleName = model.MiddleName,
            LastName = model.LastName,
            OfficeId = model.OfficeId,
            OrediggerId = model.OrediggerId,
            UserName = model.OrediggerId,
            Id = model.OrediggerId
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded) {
            await signInManager.SignInAsync(user, false);

            var response = new LoggedInModel {
                Token = tokenService.BuildToken(user),
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrediggerId = model.OrediggerId
            };

            return Ok(response);
        }

        return Conflict(result.Errors.ToList());
    }

}