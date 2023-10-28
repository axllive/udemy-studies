using API.Data.Repositories;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, 
            IConfiguration configuration)
            {                                
                services.AddDbContext<DBContext>( options => 
                { 
                    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                });

                services.AddCors(); //for accepting the request in different server sources

                //adição de serviços
                services.AddScoped<ITokenService, TokenService>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.Configure<CloudnarySettings>(configuration.GetSection("CloudnarySettings"));
                services.AddScoped<IPhotoService, PhotoService>();
                return services;
            }
    }
}