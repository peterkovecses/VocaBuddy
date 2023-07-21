using Microsoft.AspNetCore.Components;

namespace VocaBuddy.UI.Models;

public class CustomComponentBase : ComponentBase
{
    [Inject] 
    protected NavigationManager NavManager { get; set; }
}
