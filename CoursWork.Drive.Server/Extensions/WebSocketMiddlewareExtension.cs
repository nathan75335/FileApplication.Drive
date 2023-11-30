using CoursWork.Drive.BusinessLogic.Services;
using CoursWork.Drive.DataAccess.Repositories;
using CoursWork.Drive.Server.MessageHandler;
using CoursWork.Drive.Server.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CoursWork.Drive.Server.Extensions;

public static class WebSocketMiddlewareExtension
{
    public static void UseWebSocketServer(this IApplicationBuilder application)
    {
        application.UseMiddleware<WebSocketServerMiddleware>();
        application.UseMiddleware<CustomExceptionMiddleware>();
    }

    public static IServiceCollection AddWebSocketServerConnectionManager(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<WebSocketServerConnectionManager>()
            .AddScoped<JsonMessageHandler>()
            .AddScoped<IFileDriveRepository, FileDriveRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthorizeService, AuthorizeService>()
            .AddScoped<IFileDriveService, FileDriveService>()
            .AddAutoMapper(typeof(Program))
            .AddTransient<CustomExceptionMiddleware>()
            .AddScoped<IFileAccessRepository, FileAccessRepository>()
            .AddScoped<IFileAccessService, FileAccessService>();
    }


    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                RequireExpirationTime = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true
            };
        });

        return services;
    }
}
