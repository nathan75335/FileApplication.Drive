using Microsoft.Extensions.Primitives;

namespace CoursWork.Drive.BusinessLogic.DTO_s;

public class TokenDto
{
    /// <summary>
    /// The token to get authorization
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// The validity time in minutes
    /// </summary>
    public int TokenLifeTimeInMinutes { get; set; }
    public int UserId { get; set; }
    public string Email { get;set; }
}
