using System.Text;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
                IConfiguration configuration){

                services.AddIdentityCore<AppUser>( opt => {
                    opt.Password.RequireNonAlphanumeric = false;
                })  .AddRoles<AppRole>()
                    .AddRoleManager<RoleManager<AppRole>>()
                    .AddEntityFrameworkStores<DBContext>();
                    
                //injeção do serviço Jwt e seus parâmetros de configurações
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(
                        options => options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(configuration["TokenKey"])
                                ),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        }
                    );
                    
                    services.AddAuthorization(options => {
                        options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                        options.AddPolicy("ModeratorPhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
                    });

                    return services;
                }
    }
}