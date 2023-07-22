﻿using System.ComponentModel.DataAnnotations;

namespace VocaBuddy.Identity.Models;

public class UserRegistrationRequest
{
    [EmailAddress]
    public required string Email { get; set; } = default!;

    public required string Password { get; set; } = default!;
}
