using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VocaBuddy.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private readonly IMediator _mediator;

    protected IMediator Mediator => _mediator;

    protected string? CurrentUserId => HttpContext.User.FindFirstValue("Id");

    protected ApiControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }
}
