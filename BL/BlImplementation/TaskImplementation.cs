using BlApi;
using BO;

namespace BlImplementation
{
    /// <summary>
    /// Implementation of the <see cref="ITask"/> interface.
    /// </summary>
    internal class TaskImplementation : ITask
    {
        private DalApi.IDal _dal = DalApi.Factory.Get;

        /// <summary>
        /// Creates a new task in the system.
        /// </summary>
        /// <param name="boTask">The details of the new task to create.</param>
        /// <returns>The unique identifier of the newly created task.</returns>
        public int Create(BO.Task boTask)
        {
            DO.Task doTask = new DO.Task(
                boTask.Id,
                boTask.Alias,
                boTask.Description,
                boTask.CreatedAtDate,
                boTask.RequiredEffortTime,
                false,
                boTask.StartDate,
                boTask.ScheduledDate,
                boTask.DeadlineDate,
                boTask.CompleteDate,
                boTask.Deliverables,
                boTask.Remarks,
                null,
                (DO.EngineerExperience?)boTask.Copmlexity);

            var dependenciesToCreate = boTask.Dependencies != null ? boTask.Dependencies
               .Select(task => new DO.Dependency
               {
                   DependentTask = boTask.Id,
                   DependsOnTask = task.Id
               })
               .ToList() : (List<DO.Dependency>?)null;
            // יצירת כל תלות המשימה באמצעות ה-Dependency ב-DAL
            if (dependenciesToCreate != null)
            {
                dependenciesToCreate.ForEach(dependency => _dal.Dependency.Create(dependency));
            }
            try
            {
                int idTask = _dal.Task.Create(doTask);
                return idTask;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlAlreadyExistsException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Deletes a task from the system based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        public void Delete(int id)
        {
            try
            {
                _dal.Task.Delete(id);
            }
            catch (DO.DalDeletionImpossible ex)
            {
                throw new BO.BlDeletionImpossible(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads details of a task from the system based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the task to read.</param>
        /// <returns>The details of the task.</returns>
        public BO.Task? Read(int id)
        {
            DO.Task? doTask = _dal.Task.Read(id);
            if (doTask == null)
                throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

            return new BO.Task()
            {
                Id = id,
                Alias = doTask.Alias,
                Description = doTask.Description,
                CreatedAtDate = doTask.CreatedAtDate,
                Status = ReadStatus(id),
                Dependencies = ReadDependencies(id),
                Milestone = null, // Requires calculation
                RequiredEffortTime = doTask.RequiredEffortTime,
                StartDate = doTask.StartDate,
                ScheduledDate = doTask.ScheduledDate,
                ForecastDate = doTask.StartDate + doTask.RequiredEffortTime,
                DeadlineDate = doTask.DeadlineDate,
                CompleteDate = doTask.CompleteDate,
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                Engineer = ReadEngineerInTask(doTask.EngineerId),
                Copmlexity = (BO.EngineerExperience?)doTask.Copmlexity
            };
        }

        public BO.Task? Read(Func<BO.Task, bool> filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves all tasks from the system that match the specified filter criteria, or all tasks if no filter is provided.
        /// </summary>
        /// <param name="filter">Optional filter predicate to apply to the tasks.</param>
        /// <returns>An enumerable collection of <see cref="BO.Task"/> objects.</returns>
        public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
        {
            Func<BO.Task, bool> filter1 = filter != null ? filter! : item => true;

            return (from DO.Task doTask in _dal.Task.ReadAll()
                    select new BO.Task()
                    {
                        Id = doTask.Id,
                        Alias = doTask.Alias,
                        Description = doTask.Description,
                        CreatedAtDate = doTask.CreatedAtDate,
                        Status = ReadStatus(doTask.Id),
                        Dependencies = ReadDependencies(doTask.Id),
                        Milestone = null, // Requires calculation
                        RequiredEffortTime = doTask.RequiredEffortTime,
                        StartDate = doTask.StartDate,
                        ScheduledDate = doTask.ScheduledDate,
                        ForecastDate = doTask.StartDate + doTask.RequiredEffortTime,
                        DeadlineDate = doTask.DeadlineDate,
                        CompleteDate = doTask.CompleteDate,
                        Deliverables = doTask.Deliverables,
                        Remarks = doTask.Remarks,
                        Engineer = ReadEngineerInTask(doTask.EngineerId),
                        Copmlexity = (BO.EngineerExperience?)doTask.Copmlexity
                    }).Where(filter1);
        }

        /// <summary>
        /// Updates details of a task in the system.
        /// </summary>
        /// <param name="boTask">The updated details of the task.</param>
        public void Update(BO.Task boTask)
        {
            if (Read(boTask.Id) is null)
                throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} does Not exist");

            DO.Task doTask = new DO.Task(
                boTask.Id,
                boTask.Alias,
                boTask.Description,
                boTask.CreatedAtDate,
                boTask.RequiredEffortTime,
                false,
                boTask.StartDate,
                boTask.ScheduledDate,
                boTask.DeadlineDate,
                boTask.CompleteDate,
                boTask.Deliverables,
                boTask.Remarks,
                boTask.Engineer?.Id,
                (DO.EngineerExperience?)boTask.Copmlexity);

            foreach (var dependency in _dal.Dependency.ReadAll(d => d.DependentTask == boTask.Id))
            {
                _dal.Dependency.Delete(dependency.Id);
            }

            // יצירת תלות חדשות על פי התלות שהוגדרו ב-BO
            if (boTask.Dependencies != null)
            {
                foreach (TaskInList doDependency in boTask.Dependencies)
                {
                    DO.Dependency doDepend = new DO.Dependency(0, boTask.Id, doDependency.Id);
                    int idDependency = _dal.Dependency.Create(doDepend);
                }
            }
            try
            {
                _dal.Task.Update(doTask);
            }
            catch (Exception ex)
            {
                throw new BO.BlDoesNotExistException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads details of the engineer associated with the task.
        /// </summary>
        /// <param name="engineerId">The ID of the engineer associated with the task.</param>
        /// <returns>The details of the engineer associated with the task.</returns>
        public EngineerInTask? ReadEngineerInTask(int? engineerId)
        {
            if (engineerId == null)
                return null;

            var doEngineer = _dal.Engineer.ReadAll()
                .FirstOrDefault(doEngineer => doEngineer.Id == engineerId);

            if (doEngineer == null)
                return null;

            return new EngineerInTask
            {
                Id = doEngineer.Id,
                Name = doEngineer.Name
            };
        }

        /// <summary>
        /// Reads dependencies of a task from the system based on the provided task ID.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>A list of dependencies of the task.</returns>
        public List<TaskInList>? ReadDependencies(int taskId)
        {
            return (
                from DO.Dependency doDependency in _dal.Dependency.ReadAll()
                where (doDependency.DependentTask == taskId)
                let dependentTask = _dal.Task.Read(doDependency.DependsOnTask)
                select new BO.TaskInList()
                {
                    Id = doDependency.DependsOnTask,
                    Description = dependentTask.Description,
                    Alias = dependentTask.Alias,
                    Status = ReadStatus(doDependency.DependsOnTask),
                }).ToList();
        }

        /// <summary>
        /// Determines the status of a task based on its details.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The status of the task.</returns>
        public BO.Status ReadStatus(int taskId)
        {
            DO.Task? doTask = _dal.Task.Read(taskId);

            DateTime today = DateTime.Today;

            return doTask.CompleteDate.HasValue && doTask.CompleteDate.Value <= today
                ? Status.Done
                : doTask.DeadlineDate.HasValue && doTask.DeadlineDate.Value.AddDays(-7) <= today
                    ? Status.InJeopardy
                    : doTask.StartDate.HasValue && doTask.StartDate.Value <= today
                        ? Status.OnTrack
                        : doTask.ScheduledDate.HasValue
                            ? Status.Scheduled
                            : Status.Unscheduled;
        }
    }
}
