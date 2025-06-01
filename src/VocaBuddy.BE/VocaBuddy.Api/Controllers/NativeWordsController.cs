using VocaBuddy.Application.Features.NativeWord.Commands.Create;
using VocaBuddy.Application.Features.NativeWord.Commands.Delete;
using VocaBuddy.Application.Features.NativeWord.Commands.RecordMistakes;
using VocaBuddy.Application.Features.NativeWord.Commands.Update;
using VocaBuddy.Application.Features.NativeWord.Queries.GetAll;
using VocaBuddy.Application.Features.NativeWord.Queries.GetById;
using VocaBuddy.Application.Features.NativeWord.Queries.GetCount;
using VocaBuddy.Application.Features.NativeWord.Queries.GetLatest;
using VocaBuddy.Application.Features.NativeWord.Queries.GetMistaken;
using VocaBuddy.Application.Features.NativeWord.Queries.GetRandom;

namespace VocaBuddy.Api.Controllers;

[Authorize]
[Route("api/native-words")]
[ApiController]
public class NativeWordsController(IMediator mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    public async Task<IActionResult> GetNativeWords(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetNativeWordsQuery(), cancellationToken);

        return result.ToApiResponse();
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandomNativeWords(int count, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetRandomNativeWordsQuery(count), cancellationToken);

        return result.ToApiResponse();
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestNativeWords(int count, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetLatestNativeWordsQuery(count), cancellationToken);

        return result.ToApiResponse();
    }
    
    [HttpGet("mistaken")]
    public async Task<IActionResult> GetMistakenNativeWords(int count, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetMistakenNativeWordsQuery(count), cancellationToken);

        return result.ToApiResponse();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetNativeWord(int id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetNativeWordByIdQuery(id), cancellationToken);

        return result.ToApiResponse();
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetNativeWordCount(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetNativeWordCountQuery(), cancellationToken);

        return result.ToApiResponse();
    }

    [HttpPost]
    public async Task<IActionResult> CreateNativeWorld(CreateNativeWordDto nativeWord, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateNativeWordCommand(nativeWord), cancellationToken);

        return result
            .ToApiResponse(r => CreatedAtAction(
                nameof(GetNativeWord),
                new { id = r.Data!.Id },
                r));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateNativeWord(int id, UpdateNativeWordDto nativeWord, CancellationToken cancellationToken)
    {
        if (id != nativeWord.Id)
        {
            return BadRequest(Result.Failure(new ErrorInfo(VocaBuddyErrorCodes.ModelError,
                [new ApplicationError("The model id does not match the route id.")])));
        }

        var result = await Mediator.Send(new UpdateNativeWordCommand(nativeWord, id), cancellationToken);

        return result.ToApiResponse();
    }

    [HttpPut]
    public async Task<IActionResult> RecordMistakes(IEnumerable<int> mistakenWordIds, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new RecordMistakesCommand(mistakenWordIds), cancellationToken);

        return result.ToApiResponse();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteNativeWord(int id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteNativeWordCommand(id), cancellationToken);

        return result.ToApiResponse();
    }
}
