﻿using Microsoft.EntityFrameworkCore;
using RPS.Core.Interfaces;
using TicTacToe.Core.Interfaces;
using TicTacToe.MediatR;

namespace RPS.Core.Requests.Game.GetGames;

public class GetGamesQueryHandler : IRequestHandler<GetGamesQuery, GetGamesResponse>
{
    private readonly IDbContext _dbContext;

    public GetGamesQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetGamesResponse> Handle(GetGamesQuery request, CancellationToken cancellationToken)
    {
        var gamesNotFinishedAndNew = await _dbContext.Games
            .Where(g => g.IsFinished == false)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken: cancellationToken);
        
        var gamesFinished = await _dbContext.Games
            .Where(g => g.IsFinished == true)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken: cancellationToken);
        
        var unionGames = gamesNotFinishedAndNew.Union(gamesFinished);
        
        var result = unionGames
            .OrderBy(x => x.CreatedAt)
            .Select(x => new GetGameResponseItem
            {
                GameId = x.Id,
                CreateUsername = x.WhoCreatedName,
                Status = x.Status.ToString(),
                CreatedUserId = null,
            })
            .ToList();

        return new GetGamesResponse()
        {
            Items = result,
        };
    }
}