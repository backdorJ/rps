﻿using Microsoft.EntityFrameworkCore;
using RPS.Domain.Entities;
using TicTacToe.Domain.Entities;

namespace RPS.Core.Interfaces;

public interface IDbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Game> Games { get; set; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}