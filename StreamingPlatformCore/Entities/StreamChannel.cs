using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

public class StreamChannel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int AuthorId { get; private set; }
    public int CategoryId { get; private set; }
    public ulong SubscribersCount { get; private set; }

    [ForeignKey(nameof(AuthorId))]
    public User Author { get; private set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; private set; }

    public StreamChannel(string name, int authorId, int categoryId, ulong subscribersCount)
    {
        Name = name;
        AuthorId = authorId;
        CategoryId = categoryId;
        SubscribersCount = subscribersCount;
    }
}