using API.DTOs;
using API.Extensions;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.RegisterMaps();
//build the application
var app = builder.Build();

#region Middlewares

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
//add the different server source that is acceptable the request for the API
app.UseCors(builder => 
    builder.AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("https://localhost:4200", "http://localhost:4200")
);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
