namespace VocaBuddy.Shared.Models;

public class CustomTokenValidationParameters
{
    public bool ValidateIssuer { get; init; }
    public bool ValidateAudience { get; init; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateIssuerSigningKey { get; init; }
    public string ValidIssuer { get; init; } = default!;
    public string ValidAudience { get; init; } = default!;
    public string Secret { get; init; } = default!;
    public string TokenLifeTime { get; init; } = default!;
    public string? ClockSkew { get; init; }


    public TokenValidationParameters ToTokenValidationParameters() =>
        new()
        {
            ValidateIssuer = this.ValidateIssuer,
            ValidateAudience = this.ValidateAudience,
            ValidateLifetime = this.ValidateLifetime,
            ValidateIssuerSigningKey = this.ValidateIssuerSigningKey,
            ValidIssuer = this.ValidIssuer,
            ValidAudience = this.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Secret)),            
            ClockSkew = this.ClockSkew is not null ? TimeSpan.Parse(this.ClockSkew) : default
        };
}
