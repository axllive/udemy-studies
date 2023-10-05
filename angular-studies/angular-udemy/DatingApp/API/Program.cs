using API;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DBContext>( options => 
{ 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
 });

builder.Services.AddControllers();

builder.Services.AddCors(); //for accepting the request in different server sources

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();
//add the different server source that is acceptable the request for the API
app.UseCors(builder => 
    builder.AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200")
);

app.MapControllers();

app.Run();
