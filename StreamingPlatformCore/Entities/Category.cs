using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

public class Category
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public string Name { get; private set; }

    public Category(string name)
    {
        Name = name;
    }
}