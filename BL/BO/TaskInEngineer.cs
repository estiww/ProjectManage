
namespace BO;

/// <summary>
/// Represents a task assigned to an engineer.
/// </summary>
public class TaskInEngineer
{
   public int Id { get; init; }
   public string Alias{ get; set; }

    /// <summary>
    /// Returns a string that represents the current engineer.
    /// </summary>
    /// <returns>A string representation of the engineer.</returns>
    public override string ToString()
    {
        return Tools.GenericToString(this);
    }
} 
