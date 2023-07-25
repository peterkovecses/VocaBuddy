using MediatR;
using Microsoft.AspNetCore.Mvc;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;

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
        return Ok(await _mediator.Send(new GetNativeWordsQuery(), token));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNativeWord(int id, CancellationToken token)
    {
        return Ok(await _mediator.Send(new GetNativeWordByIdQuery(id), token));
    }

    [HttpPost]
    public async Task<IActionResult> CreateNativeWorld(NativeWordDto nativeWord, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdNativeWordDto = await _mediator.Send(new InsertNativeWordCommand(nativeWord), token);

        return CreatedAtAction(nameof(GetNativeWord), new { id = createdNativeWordDto.Id }, createdNativeWordDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNativeWord(int id, NativeWordDto nativeWord)
    {
        if (id != nativeWord.Id)
        {
            ModelState.AddModelError("id", "The specified id does not match the id of the object to be modified.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _mediator.Send(new UpdateNativeWordCommand(nativeWord));

        return Ok();
    }
}
