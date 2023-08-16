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
    public async Task<IActionResult> GetNativeWords(CancellationToken token)
    {
        var words = await Mediator.Send(new GetNativeWordsQuery(CurrentUserId!), token);

        return Ok(Result.Success(words));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNativeWord(int id, CancellationToken token)
    {
        var word = await Mediator.Send(new GetNativeWordByIdQuery(id, CurrentUserId!), token);

        return Ok(Result.Success(word));
    }

    [HttpPost]
    public async Task<IActionResult> CreateNativeWorld(NativeWordDto nativeWord, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdNativeWordDto = await Mediator.Send(new InsertNativeWordCommand(nativeWord, CurrentUserId!), token);

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

        await Mediator.Send(new UpdateNativeWordCommand(nativeWord, CurrentUserId!), token);

        return Ok(Result.Success());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNativeWord(int id, CancellationToken token)
    {
        await Mediator.Send(new DeleteNativeWordCommand(id, CurrentUserId!), token);

        return Ok(Result.Success());
    }
}
