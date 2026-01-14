using OneMore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMore.Infra.Data.Repositories;

public class WordRepository(DataContext context) : IWordRepository
{
    private readonly DataContext _context = context;
    public async Task<string> GetRandomWordAsync()
        => _context.Words
            .OrderBy(r => Guid.NewGuid())
            .Select(w => w.Text)
            .FirstOrDefault() ?? string.Empty;
}
