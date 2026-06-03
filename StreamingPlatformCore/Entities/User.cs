using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Represents user in the system, can own multiple stream channels, send messages, donations and make subscriptions
/// </summary>
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; } // who cares about security

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; }
    public virtual ICollection<Donation> Donations { get; set; }
    public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    public virtual ICollection<StreamChannel> OwnedChannels { get; set; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
}