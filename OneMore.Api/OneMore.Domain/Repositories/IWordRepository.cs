using System;
using System.Collections.Generic;
using System.Text;

namespace OneMore.Domain.Repositories;

public interface IWordRepository
{
    Task<string> GetRandomWordAsync();
}
