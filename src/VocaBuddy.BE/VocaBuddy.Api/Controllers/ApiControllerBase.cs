using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace VocaBuddy.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected IMediator Mediator { get; }

    protected ApiControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }
}
