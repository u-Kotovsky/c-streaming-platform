using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamingPlatformCore.Entities;

/// <summary>
/// Категория трансляции (жанр, тематика).
/// </summary>
public class Category
{
    /// <summary>
    /// Уникальный идентификатор категории.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Название категории (например, "Игры", "Музыка", "Общение").
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Краткое описание категории.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Создаёт новый экземпляр категории.
    /// </summary>
    /// <param name="name">Название категории.</param>
    /// <param name="description">Описание категории.</param>
    public Category(string name, string description)
    {
        Name = name;
        Description = description;
    }
}