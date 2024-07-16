namespace BO;

/// <summary>
/// Enumerates the possible experience levels of an engineer.
/// </summary>
public enum EngineerExperience
{
    None,
    Novice,
    AdvancedBeginner,
    Competent,
    Proficient,
    Expert
}

/// <summary>
/// Enumerates the possible statuses of a task.
/// </summary>
public enum Status
{
    Unscheduled,//לא משובץ
    Scheduled,//בלו"ז
    OnTrack,//בתהליך
    InJeopardy,//דחוף!
    Done,//נעשתה
}
