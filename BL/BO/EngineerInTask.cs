
namespace BO
{
    /// <summary>
    /// Represents an engineer involved in a task.
    /// </summary>
    public class EngineerInTask
    {
        public int Id { get; init; }
        public string Name { get; set; }

        /// <summary>
        /// Returns a string that represents the current engineer involved in a task.
        /// </summary>
        /// <returns>A string representation of the engineer in the task.</returns>
        public override string ToString()
        {
            return Tools.GenericToString(this);
        }
    }
}
