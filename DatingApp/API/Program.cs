using System.Text.Json.Serialization;
using API;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
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

using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DBContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
        await context.Database.MigrateAsync();
        await Seed.SeedUsers(userManager, roleManager);
        
        
    }
    catch (System.Exception ex)
    {
        var logger = services.GetService<ILogger<Program>>();
        logger.LogError(ex, "an error occurred during migration");
    }

#endregion

app.Run();
