using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneMore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneMore.Infra.Data.Config;

public class WordConfig : IEntityTypeConfiguration<Word>
{
    public void Configure(EntityTypeBuilder<Word> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Category).IsRequired().HasMaxLength(100);
        builder.Property(w => w.Text).IsRequired().HasMaxLength(200);
    }
}
