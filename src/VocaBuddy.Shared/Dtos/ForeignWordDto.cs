﻿namespace VocaBuddy.Shared.Dtos;

public class ForeignWordDto
{
    public int Id { get; set; }
    public string Text { get; set; }

    public int NativeWordId { get; set; }
}
