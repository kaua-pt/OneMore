using OneMore.Domain.Entities.Abstract;

namespace OneMore.Domain.Entities;

public class Word : Entity
{
    public string Category { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
