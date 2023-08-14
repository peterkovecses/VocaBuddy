using MediatR;
using Microsoft.AspNetCore.Mvc;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Api.Controllers;

[Route("api/native-words")]
[ApiController]
public class NativeWordsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NativeWordsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetNativeWords(CancellationToken token)
    {
        // temporary hard coded user id
        var userId = "4c27e48e-8bea-4e56-8ab5-48d8ab88e6b5";
        var words = await _mediator.Send(new GetNativeWordsQuery(userId), token);

        return Ok(Result.Success(words));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNativeWord(int id, CancellationToken token)
    {
        var word = await _mediator.Send(new GetNativeWordByIdQuery(id), token);

        return Ok(Result.Success(word));
    }

    [HttpPost]
    public async Task<IActionResult> CreateNativeWorld(NativeWordDto nativeWord, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdNativeWordDto = await _mediator.Send(new InsertNativeWordCommand(nativeWord), token);

        return CreatedAtAction(nameof(GetNativeWord), new { id = createdNativeWordDto.Id }, Result.Success(createdNativeWordDto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNativeWord(int id, NativeWordDto nativeWord, CancellationToken token)
    {
        if (id != nativeWord.Id)
        {
            ModelState.AddModelError("id", "The specified id does not match the id of the object to be modified.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _mediator.Send(new UpdateNativeWordCommand(nativeWord), token);

        return Ok(Result.Success());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNativeWord(int id, CancellationToken token)
    {
        await _mediator.Send(new DeleteNativeWordCommand(id), token);

        return Ok(Result.Success());
    }
}
