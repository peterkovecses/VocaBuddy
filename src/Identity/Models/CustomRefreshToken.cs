namespace Identity.Models
{
    public class CustomRefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Token { get; set; } = default!;
        public string JwtId { get; set; } = default!;
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public string UserId { get; set; } = default!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = default!;
    }
}
