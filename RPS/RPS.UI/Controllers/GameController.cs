﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPS.Core.Requests.Game.CreateGame;
using RPS.Core.Requests.Game.GetGame;
using RPS.Core.Requests.Game.GetGames;
using TicTacToe.Core.Requests.Game.CreateGame;
using TicTacToe.MediatR;

namespace RPS.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IMediator _mediator;

    public GameController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-game")]
    public async Task<CreateGameResponse> Handle(CreateGameRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateGameCommand(request.RoomName, request.MaxRating), cancellationToken);
        
        return new CreateGameResponse
        {
            Id = result.Id
        };
    }

    [HttpGet("get-games")]
    public async Task<GetGamesResponse> GetGames(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetGamesQuery(), cancellationToken);
        return result;
    }
    
    [HttpGet("join-game/{id}")]
    public async Task<GetGameResponse> JoinGame(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetGameQuery(id), cancellationToken);
        return result;
    }
}