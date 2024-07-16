namespace Dal;

internal static class DataSource
{
    /// <summary>
    /// Generates automatic running numbers, for the fields that are defined as "running ID number"
    /// </summary>
    internal static class Config
    {
        internal const int startTaskId = 0;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int startDependencyId = 0;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
        internal static DateTime? ProjectStartDate { get; set; } = null;
        internal static DateTime? ProjectPlannedEndDate { get; set; } = null;
       
    }

    /// <summary>
    /// For each of the DO entities defined in the previous step, we added to the DataSource class a list that would contain all references to entities of the same type.
    /// </summary>
    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static DateTime startDate = DateTime.Today;
    internal static DateTime lastDate = DateTime.Today.AddYears(1);
}



