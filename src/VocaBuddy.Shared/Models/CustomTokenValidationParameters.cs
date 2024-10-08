﻿namespace VocaBuddy.Shared.Models;

public class CustomTokenValidationParameters
{
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public string ValidIssuer { get; set; } = default!;
    public string ValidAudience { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public string TokenLifeTime { get; set; } = default!;
    public string? ClockSkew { get; set; }


    public TokenValidationParameters ToTokenValidationParameters()
    {
        return new TokenValidationParameters
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
}
