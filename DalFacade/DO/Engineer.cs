

namespace DO;
/// <summary>
/// Engineer Entity represents a engineer with all its props
/// </summary>
/// <param name="Id">Unique ID number</param>
/// <param name="Name">Engineer's name (full name)</param>
/// <param name="Email">Engineer's Email</param>
/// <param name="Level">Engineer level</param>
/// <param name="Cost">cost per hour</param>
public record Engineer
(
    int Id,    
    string Name,
    string Email,
    EngineerExperience Level,
    double Cost
)
{
    public Engineer() : this(0, "", "", 0, 0) { }
}

