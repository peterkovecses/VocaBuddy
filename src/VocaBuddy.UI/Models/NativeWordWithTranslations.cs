namespace VocaBuddy.UI.Models;

public class NativeWordWithTranslations
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public required string UserId { get; set; }
    public DateTime CreatedUtc { get; set; }
    public required string TranslationsString { get; set; }
}
