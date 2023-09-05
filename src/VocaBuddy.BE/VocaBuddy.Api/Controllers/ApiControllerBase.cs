using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace VocaBuddy.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private readonly IMediator _mediator;

    protected IMediator Mediator => _mediator;

    protected ApiControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }
}
