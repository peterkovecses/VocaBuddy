namespace Identity.Application.Features.Refresh;

public class CustomRefreshToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Token { get; init; } = null!;
    public required string JwtId { get; init; }
    public DateTime CreationDate { get; init; }
    public DateTime ExpiryDate { get; init; }
    public bool Used { get; set; }
    public bool Invalidated { get; init; }
    public required string UserId { get; init; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; init; } = null!;
}