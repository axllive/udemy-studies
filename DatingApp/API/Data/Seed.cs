using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            var roles = new List<AppRole>
            {
                new AppRole{Name="Member"},
                new AppRole{Name="Admin"},
                new AppRole{Name="Moderator"},
            };

            foreach (var item in roles)
            {
                await roleManager.CreateAsync(item);    
            }

            foreach (var usr in users)
            {

                usr.UserName = usr.UserName.ToLower();
                usr.Bio = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium.";
                
                await userManager.CreateAsync(usr, "Pas$$w0rd");
                await userManager.AddToRoleAsync(usr, "Member");
            }

            var admin = new AppUser{
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});

        }
    }
}