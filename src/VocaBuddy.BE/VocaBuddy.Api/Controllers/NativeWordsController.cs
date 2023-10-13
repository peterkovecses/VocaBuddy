using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VocaBuddy.Api.Extensions;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Api.Controllers;

[Authorize]
[Route("api/native-words")]
[ApiController]
public class NativeWordsController : ApiControllerBase
{

    public NativeWordsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetNativeWords(int? randomItemCount, CancellationToken token)
    {
        var result = await Mediator.Send(new GetNativeWordsQuery(randomItemCount), token);

        return result.ToApiResponse();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNativeWord(int id, CancellationToken token)
    {
        var result = await Mediator.Send(new GetNativeWordByIdQuery(id), token);

        return result.ToApiResponse();
    }

    [HttpPost]
    public async Task<IActionResult> CreateNativeWorld(NativeWordDto nativeWord, CancellationToken token)
    {
        var result = await Mediator.Send(new CreateNativeWordCommand(nativeWord), token);

        return result
            .ToApiResponse(result => CreatedAtAction(
                nameof(GetNativeWord),
                new { id = result.Data! },
                result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNativeWord(int id, NativeWordDto nativeWord, CancellationToken token)
    {
        if (id != nativeWord.Id)
        {
            return BadRequest(Result.Failure(new(VocaBuddyErrorCodes.ModelError, new[] { new ApplicationError("The model id does not match the route id.") })));
        }

        var result = await Mediator.Send(new UpdateNativeWordCommand(nativeWord, id), token);

        return result.ToApiResponse();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNativeWord(int id, CancellationToken token)
    {
        var result = await Mediator.Send(new DeleteNativeWordCommand(id), token);

        return result.ToApiResponse();
    }
}
