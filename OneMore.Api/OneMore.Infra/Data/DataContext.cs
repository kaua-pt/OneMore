using Microsoft.EntityFrameworkCore;
using OneMore.Domain.Entities;
using OneMore.Infra.Data.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMore.Infra.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Word> Words => Set<Word>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WordConfig());
    }
}
