﻿namespace VocaBuddy.Shared.Dtos;

public class NativeWordDto
{
    public int Id { get; set; }
    public string Text { get; set; } = default!;
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }

    public List<ForeignWordDto> Translations { get; set; } = default!;
}
