using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DBContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            foreach (var usr in users)
            {
                using var hmac = new HMACSHA512();

                usr.UserName = usr.UserName.ToLower();
                usr.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("fuckingP$wd"));
                usr.PasswordSalt = hmac.Key;
                usr.Bio = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium.";
                
                context.Users.Add(usr);
            }

            await context.SaveChangesAsync();
        }
    }
}