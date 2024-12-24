namespace VocaBuddy.Api.Controllers;

[Authorize]
[Route("api/native-words")]
[ApiController]
public class NativeWordsController(IMediator mediator) : ApiControllerBase(mediator)
{
    [HttpGet]
    public async Task<IActionResult> GetNativeWords(CancellationToken token)
    {
        var result = await Mediator.Send(new GetNativeWordsQuery(), token);

        return result.ToApiResponse();
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandomNativeWords(int count, CancellationToken token)
    {
        var result = await Mediator.Send(new GetRandomNativeWordsQuery(count), token);

        return result.ToApiResponse();
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestNativeWords(int count, CancellationToken token)
    {
        var result = await Mediator.Send(new GetLatestNativeWordsQuery(count), token);

        return result.ToApiResponse();
    }
    
    [HttpGet("mistaken")]
    public async Task<IActionResult> GetMistakenNativeWords(int count, CancellationToken token)
    {
        var result = await Mediator.Send(new GetMistakenNativeWordsQuery(count), token);

        return result.ToApiResponse();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetNativeWord(int id, CancellationToken token)
    {
        var result = await Mediator.Send(new GetNativeWordByIdQuery(id), token);

        return result.ToApiResponse();
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetNativeWordCount(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetNativeWordCountQuery(), cancellationToken);

        return result.ToApiResponse();
    }

    [HttpPost]
    public async Task<IActionResult> CreateNativeWorld(CompactNativeWordDto nativeWord, CancellationToken token)
    {
        var result = await Mediator.Send(new CreateNativeWordCommand(nativeWord), token);

        return result
            .ToApiResponse(r => CreatedAtAction(
                nameof(GetNativeWord),
                new { id = r.Data! },
                r));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateNativeWord(int id, CompactNativeWordDto nativeWord, CancellationToken token)
    {
        if (id != nativeWord.Id)
        {
            return BadRequest(Result.Failure(new ErrorInfo(VocaBuddyErrorCodes.ModelError, [new ApplicationError("The model id does not match the route id.")])));
        }

        var result = await Mediator.Send(new UpdateNativeWordCommand(nativeWord, id), token);

        return result.ToApiResponse();
    }

    [HttpPut]
    public async Task<IActionResult> RecordMistakes(IEnumerable<int> mistakenWordIds, CancellationToken token)
    {
        var result = await Mediator.Send(new RecordMistakesCommand(mistakenWordIds), token);

        return result.ToApiResponse();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteNativeWord(int id, CancellationToken token)
    {
        var result = await Mediator.Send(new DeleteNativeWordCommand(id), token);

        return result.ToApiResponse();
    }
}
