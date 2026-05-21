using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    [Required]
    public string Login { get; private set; }
    [Required]
    public string Password { get; private set; } // who cares about securite
    public DateTime CreatedAt { get; private set; }

    public User(string login, string password)
    {
        CreatedAt = DateTime.UtcNow;
        Login = login;
        Password = password;
    }
}