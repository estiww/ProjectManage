

namespace BlApi
{
    /// <summary>
    /// Represents the business logic interface for managing tasks, engineers, and milestones.
    /// </summary>
    public interface IBl
    {
        /// <summary>
        /// Gets the interface for managing tasks.
        /// </summary>
        public ITask Task { get; }

        /// <summary>
        /// Gets the interface for managing engineers.
        /// </summary>
        public IEngineer Engineer { get; }

        /// <summary>
        /// Gets the interface for managing milestones.
        /// </summary>
        public IMilestone Milestone { get; }
    }
}
