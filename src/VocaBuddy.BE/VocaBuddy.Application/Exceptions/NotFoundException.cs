namespace VocaBuddy.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(int id) : base($"Item with id {id} not found.")
    {
    }
}
