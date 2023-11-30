using CoursWork.Drive.DataAccess;
using CoursWork.Drive.Server.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DriveContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Sql"));
        options.EnableSensitiveDataLogging();
    }, ServiceLifetime.Transient)
    .AddDbContextFactory<DriveContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Sql")), ServiceLifetime.Scoped)
    .AddWebSocketServerConnectionManager(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddAuthorization()
    .AddSwaggerGen()
    .AddEndpointsApiExplorer()
    .AddControllers();
    

builder.Services.AddCors(policy => policy.AddPolicy("Any", options =>
{
    options
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .AllowAnyHeader();
}));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Any");
app.UseAuthentication();

app.UseAuthorization();

app.UseWebSockets();

app.UseWebSocketServer();

app.MapControllers();

app.Run();
