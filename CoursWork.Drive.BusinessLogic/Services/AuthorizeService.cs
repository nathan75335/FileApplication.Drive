using CoursWork.Drive.BusinessLogic.DTO_s;
using CoursWork.Drive.BusinessLogic.Exceptions;
using CoursWork.Drive.DataAccess.Entities;
using CoursWork.Drive.DataAccess.Repositories;
using CoursWork.Drive.Shared.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoursWork.Drive.BusinessLogic.Services;

public class AuthorizeService : IAuthorizeService
{
    private readonly IConfiguration _settings;
    private readonly ILogger<AuthorizeService> _logger;
    private readonly IUserRepository _userRepository;

    public AuthorizeService(IConfiguration settings, ILogger<AuthorizeService> logger, IUserRepository userRepository)
    {
        _settings = settings;
        _logger = logger;
        _userRepository = userRepository;
    }

    /// <summary>
    ///<inheritdoc/>
    /// </summary>
    /// <param name="email">The email of the user that we want to authorize</param>
    /// <param name="password">The password of the user</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <exception cref="NotFoundException">When the user is not found</exception>
    /// <returns>A Task That contains a Token</returns>
    public async Task<UserSession> AuthorizeAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await ValidateUserAsync(email, password, cancellationToken);
        var token = GenerateTokenAsync(user);

        return new UserSession
        {
            UserId = user.Id,
            ExpiresInMinutes = Convert.ToInt32(_settings.GetSection("JWT")["LifeTimeRefresh"]),
            Token = token,
            Email = user.Email,
            Role = user.Role.Name
        };
    }

    /// <summary>
    /// Function To generate the token from the claims of the user
    /// </summary>
    /// <param name="user">The user</param>
    /// <exception cref="NotFoundException">When the user is not found</exception>
    /// <returns>A token as a <see cref="string"/> </returns>
    private string GenerateTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.GetSection("JWT")["Key"]);
      
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.Role, "User"),
            }),

            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_settings.GetSection("JWT")["LifeTime"])),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature),
            Audience = _settings.GetSection("JWT")["Audience"],
            Issuer = _settings.GetSection("JWT")["Issuer"]
        };

        try
        {
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occured while creating the token");

            throw new NotFoundException("The token can not created " + ex.Message);
        }
    }

    public async Task<User> ValidateUserAsync(string email, string password, CancellationToken cancellationToken)
    {
        var userChecked = await _userRepository.CheckPasswordAsync(email, password, cancellationToken);

        if (userChecked is null )
        {
            _logger.LogError("Error occured while processing the validation of credentials ");

            throw new NotFoundException("The user was not found check your password or your email");
        }

        return userChecked;
    }
}
