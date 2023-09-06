using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;
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
        var words = await Mediator.Send(new GetNativeWordsQuery(randomItemCount), token);

        return Ok(Result.Success(words));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNativeWord(int id, CancellationToken token)
    {
        var word = await Mediator.Send(new GetNativeWordByIdQuery(id), token);

        return Ok(Result.Success(word));
    }

    [HttpPost]
    public async Task<IActionResult> CreateNativeWorld(NativeWordDto nativeWord, CancellationToken token)
    {
        var createdNativeWordDto = await Mediator.Send(new CreateNativeWordCommand(nativeWord), token);

        return CreatedAtAction(nameof(GetNativeWord), new { id = createdNativeWordDto.Id }, Result.Success(createdNativeWordDto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNativeWord(int id, NativeWordDto nativeWord, CancellationToken token)
    {
        await Mediator.Send(new UpdateNativeWordCommand(nativeWord, id), token);

        return Ok(Result.Success());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNativeWord(int id, CancellationToken token)
    {
        await Mediator.Send(new DeleteNativeWordCommand(id), token);

        return Ok(Result.Success());
    }
}
