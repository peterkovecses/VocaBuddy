﻿namespace VocaBuddy.UI.Models;

public class NativeWordListViewModel
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public required string TranslationsString { get; set; }
}
