using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string UserName {get; set;}
    [StringLength(100)]
    public string Bio {get ; set;}
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}
