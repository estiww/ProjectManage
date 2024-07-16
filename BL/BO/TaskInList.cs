namespace BO;

/// <summary>
/// Represents a task in a list with basic information.
/// </summary
public class TaskInList
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public Status? Status { get; set; }

    /// <summary>
    /// Returns a string that represents the current engineer.
    /// </summary>
    /// <returns>A string representation of the engineer.</returns>
    public override string ToString()
    {
        return Tools.GenericToString(this);
    }
}