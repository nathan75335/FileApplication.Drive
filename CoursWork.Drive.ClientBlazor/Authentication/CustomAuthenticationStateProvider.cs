using Blazored.SessionStorage;
using CoursWork.Drive.ClientBlazor.Extension;
using CoursWork.Drive.Shared.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CoursWork.Drive.ClientBlazor.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorateService;
    private ClaimsPrincipal _principals =  new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ISessionStorageService sessionStorateService)
    {
        _sessionStorateService = sessionStorateService;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSession = await _sessionStorateService.ReadEncryptedItemAsync<UserSession>("UserSession");
            if(userSession is null)
                return await Task.FromResult(new AuthenticationState(_principals));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userSession.Email),
                new Claim(ClaimTypes.Role, userSession.Role),
                new Claim(ClaimTypes.Email, userSession.Email),
            }, "JwtAuth"));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(_principals));
        }
    }

    public async Task UpdateAuthenticationStateAsync(UserSession? userSession)
    {
        ClaimsPrincipal claimsPrincipal;

        if(userSession is not null)
        {
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userSession.Email),
                new Claim(ClaimTypes.Role, userSession.Role),
                new Claim(ClaimTypes.Email, userSession.Email),
            }, "JwtAuth"));

            userSession.ExpiryTimeStamp = DateTime.Now.AddMinutes(userSession.ExpiresInMinutes);
            await _sessionStorateService.SaveItemEncryptedAsync("UserSession", userSession);
        }
        else
        {
            claimsPrincipal = new ClaimsPrincipal(_principals);
            await _sessionStorateService.RemoveItemAsync("UserSession");
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task<UserSession> GetUserSessionAsync()
    {
        UserSession result ;

        try
        {
            var userSession = await _sessionStorateService.ReadEncryptedItemAsync<UserSession>("UserSession");
            if (userSession is not null && DateTime.Now < userSession.ExpiryTimeStamp)
                result = new UserSession();
                result = userSession;
        }
        catch 
        {
            throw new ArgumentException("An exception occured during the authentication process");
        }

        return result;
    }
}
