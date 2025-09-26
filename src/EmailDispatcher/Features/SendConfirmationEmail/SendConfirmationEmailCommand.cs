namespace EmailDispatcher.Features.SendConfirmationEmail;

public record SendConfirmationEmailCommand : IRequest<Unit>
{
    public required string Id { get; set; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string FullName => $"{FirstName} {LastName}";
    public required string ConfirmationLink { get; init; }
}