using OneMore.Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMore.Domain.Entities;

public class Player : Entity
{
    public string Name { get; set; } = string.Empty;
    public Session? Session { get; set; } = null;
}
