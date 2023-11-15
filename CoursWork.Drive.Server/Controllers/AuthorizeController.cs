using Microsoft.AspNetCore.Mvc;
using CoursWork.Drive.BusinessLogic.Services;
using CoursWork.Drive.Shared.Requests;

namespace CoursWork.Drive.Server.Controllers;

[ApiController, Route("authorize")]
public class AuthorizeController : ControllerBase
{
    private readonly IAuthorizeService _authorizationService;

    public AuthorizeController(IAuthorizeService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLogin userLogin, CancellationToken cancellationToken)
    {
        return Ok(await _authorizationService.AuthorizeAsync(userLogin.Email, userLogin.Password, cancellationToken));
    }
}
