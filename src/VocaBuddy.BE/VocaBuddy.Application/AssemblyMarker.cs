using System.Reflection;

namespace VocaBuddy.Application;

public static class ApplicationAssemblyMarker
{
    public static Assembly Assembly => typeof(ApplicationAssemblyMarker).Assembly;
}
