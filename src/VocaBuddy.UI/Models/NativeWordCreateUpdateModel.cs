namespace VocaBuddy.UI.Models;

public class NativeWordCreateUpdateModel
{
    public int Id { get; set; }
    public string Text { get; set; }
    public virtual List<ForeignWordCreateUpdateModel> Translations { get; set; }
}
