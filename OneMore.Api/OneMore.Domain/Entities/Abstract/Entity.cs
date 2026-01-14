using System;
using System.Collections.Generic;
using System.Text;

namespace OneMore.Domain.Entities.Abstract;

public abstract class Entity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}
