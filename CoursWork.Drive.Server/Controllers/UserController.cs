using CoursWork.Drive.BusinessLogic.Services;
using CoursWork.Drive.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursWork.Drive.Server.Controllers;

[ApiController, Route("users"), Authorize(Roles = "User")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRegister userRegister, CancellationToken cancellationToken)
    {
        return Ok(await _userService.RegisterAsync(userRegister, cancellationToken));
    }
}
