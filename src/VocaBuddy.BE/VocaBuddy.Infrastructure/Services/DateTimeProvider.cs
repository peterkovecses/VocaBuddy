﻿using VocaBuddy.Infrastructure.Interfaces;

namespace VocaBuddy.Infrastructure.Services;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
