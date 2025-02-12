﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPS.Domain.Entities;

namespace RPS.DAL.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.IsFinished);
        builder.Property(x => x.RoomName).HasDefaultValue("RPS Room");
        builder.Property(x => x.WinnerId);
        builder.Property(x => x.WhoCreatedName).HasDefaultValue("RPS");

        builder.HasOne(x => x.Winner)
            .WithMany();

        builder.HasMany(x => x.Users)
            .WithMany(x => x.Games);
        
        builder.HasMany(x => x.Moves)
            .WithOne(x => x.Game)
            .HasForeignKey(x => x.GameId)
            .HasPrincipalKey(x => x.Id);
    }
}