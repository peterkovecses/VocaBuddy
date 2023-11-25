using System.Reflection;

namespace VocaBuddy.UI;

public static class UiAssemblyMarker
{
    public static Assembly Assembly => typeof(UiAssemblyMarker).Assembly;
}
