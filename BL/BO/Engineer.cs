namespace BO
{
    /// <summary>
    /// Represents an engineer entity.
    /// </summary>
    /// <remarks>
    /// The Engineer class encapsulates information about an engineer, including its unique identifier, name, email address, level, hourly cost, and associated task.
    /// </remarks>
    public class Engineer
    {
        public int Id { get; init; }
        public string Name { get; set; } 
        public string Email { get; set; }
        public EngineerExperience Level { get; set; }
        public double Cost { get; set; }
        public TaskInEngineer? Task { get; set; }

        /// <summary>
        /// Returns a string that represents the current engineer.
        /// </summary>
        /// <returns>A string representation of the engineer.</returns>
        public override string ToString()
        {
            return Tools.GenericToString(this);
        }
    }
}
