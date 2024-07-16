using BlApi;
using BO;
using DalApi;
using DO;

namespace BlImplementation
{
    /// <summary>
    /// Implementation of the milestone-related business logic operations.
    /// </summary>
    internal class MilestoneImplementation : IMilestone
    {
        private readonly IDal _dal = DalApi.Factory.Get;

        /// <summary>
        /// Creates milestones based on task dependencies.
        /// </summary>
        public void Create()
        {
            // Retrieve all dependencies and group them by the dependent task.
            var groupedDependencies = _dal.Dependency.ReadAll()
                .OrderBy(dep => dep?.DependsOnTask)
                .GroupBy(dep => dep.DependentTask, dep => dep.DependsOnTask,
                         (id, dependency) => new { TaskId = id, Dependencies = dependency })
                .ToList();

            // Select distinct dependencies.
            var distinctDependencies = groupedDependencies
                .GroupBy(dep => dep.Dependencies,
                         (key, group) => group.First())
                .ToList();

            int id = 1;

            // Create milestones from distinct dependencies.
            var mileStones = distinctDependencies.Select(distinctDependency => new BO.Task()
            {
                Id = id++,
                Alias = "M" + id,
                Description = null,
                CreatedAtDate = DateTime.Today,
                Status = Status.Scheduled,
                Dependencies = (from dep in distinctDependency.Dependencies
                                let task = _dal.Task.Read(dep)
                                select new BO.TaskInList()
                                {
                                    Id = task.Id,
                                    Description = task.Description,
                                    Alias = task.Alias,
                                    Status = null,
                                }).ToList(),
                StartDate = null,
                ForecastDate = null,
                CompleteDate = null,
                DeadlineDate = null,
                Remarks = null,
                Milestone = null,
                RequiredEffortTime = null,
                Deliverables = null,
                Engineer = null,
                Copmlexity = null,
            });

            // Convert BO.Task to DO.Task and create them in the data store.
            var dalMileStones = mileStones.Select(mileStone => new DO.Task()
            {
                Id = id++,
                Alias = mileStone.Alias,
                Description = mileStone.Description,
                CreatedAtDate = mileStone.CreatedAtDate,
                RequiredEffortTime = mileStone.RequiredEffortTime,
                IsMilestone = true,
                StartDate = mileStone.StartDate,
                ScheduledDate = mileStone.ScheduledDate,
                DeadlineDate = mileStone.DeadlineDate,
                CompleteDate = mileStone.CompleteDate,
                Deliverables = mileStone.Deliverables,
                Remarks = mileStone.Remarks,
                EngineerId = null,
                Copmlexity = null,
            });

            // Create milestones in the data store.
            dalMileStones.ToList().ForEach(dalMileStone => _dal.Task.Create(dalMileStone));

            // Update dependencies with milestone IDs.
            var updatedDependencies = from mileStone in mileStones
                                      from dependency in mileStone.Dependencies
                                      from dalDependency in _dal.Dependency.ReadAll(dep => dep.DependsOnTask == dependency.Id)
                                      let dalMilestone = _dal.Task.Read(t => t.Alias == mileStone.Alias)
                                      select new Dependency(dalDependency.Id, dalDependency.DependentTask, dalMilestone.Id);

            updatedDependencies.ToList().ForEach(updatedDependency => _dal.Dependency.Update(updatedDependency));

            // Create START milestone.
            var dalStartMilestone = new DO.Task()
            {
                Id = id,
                Alias = "START",
                Description = null,
                CreatedAtDate = DateTime.Today,
                StartDate = null,
                ScheduledDate = null,
                CompleteDate = null,
                DeadlineDate = null,
                Remarks = null,
                IsMilestone = true,
                RequiredEffortTime = null,
                Deliverables = null,
                EngineerId = null,
                Copmlexity = null,
            };
            _dal.Task.Create(dalStartMilestone);

            // Get START milestone ID.
            var dalBeginMilestone = _dal.Task.Read(t => t.Alias == dalStartMilestone.Alias);

            // Create dependencies for START milestone.
            var startDependencies = from task in _dal.Task.ReadAll()
                                    let dependency = _dal.Dependency.ReadAll(dep => dep.DependentTask == task.Id)
                                    where dependency == null
                                    select _dal.Dependency.Create(new Dependency(0, task.Id, dalBeginMilestone.Id));

            // Create END milestone.
            var dalEndMilestone = new DO.Task()
            {
                Id = id,
                Alias = "END",
                Description = null,
                CreatedAtDate = DateTime.Today,
                StartDate = null,
                ScheduledDate = null,
                CompleteDate = null,
                DeadlineDate = null,
                Remarks = null,
                IsMilestone = true,
                RequiredEffortTime = null,
                Deliverables = null,
                EngineerId = null,
                Copmlexity = null,
            };

            _dal.Task.Create(dalEndMilestone);

            // Get END milestone ID.
            var dalFinishMilestone = _dal.Task.Read(t => t.Alias == dalEndMilestone.Alias);

            // Create dependencies for END milestone.
            var endDependencies = (from task in _dal.Task.ReadAll()
                                   let dependency = _dal.Dependency.ReadAll((dep => dep.DependsOnTask == task.Id))
                                   where dependency == null
                                   select new BO.TaskInList()
                                   {
                                       Id = task.Id,
                                       Description = task.Description,
                                       Alias = task.Alias,
                                       Status = null,
                                   }).ToList();

            foreach (var dependency in endDependencies)
            {
                _dal.Dependency.Create(new Dependency(0, dependency.Id, dalFinishMilestone.Id));
            }
        }

        /// <summary>
        /// Retrieves information about a milestone.
        /// </summary>
        /// <param name="id">The ID of the milestone.</param>
        /// <returns>The milestone information.</returns>
        public Milestone? Read(int id)
        {
            var milestone = _dal.Task.Read(id);
            var dependencies = _dal.Dependency.ReadAll(dep => dep.DependsOnTask == id);
            var dependenciesList = (from dependency in dependencies
                                    let task = _dal.Task.Read(dependency.DependentTask)
                                    select new BO.TaskInList()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Alias = task.Alias,
                                        Status = null,
                                    }).ToList();

            return new BO.Milestone()
            {
                Id = id,
                Description = milestone.Description,
                Alias = milestone.Alias,
                CreatedAtDate = milestone.CreatedAtDate,
                Status = Status.Scheduled,
                StartDate = null,
                ForecastDate = null,
                DeadlineDate = null,
                CompleteDate = null,
                CompletionPercentage = dependenciesList.Count(dep => dep.Status == Status.Done) / dependenciesList.Count(),
                Remarks = null,
                Dependencies = dependenciesList,
            };
        }

        /// <summary>
        /// Updates information about a milestone.
        /// </summary>
        /// <param name="milestone">The milestone to update.</param>
        public void Update(Milestone milestone)
        {
            if (Read(milestone.Id) is null)
                throw new BO.BlDoesNotExistException($"Milestone with ID={milestone.Id} does not exist");

            DO.Task doMilestone = new DO.Task
                (milestone.Id, milestone.Alias, milestone.Description, milestone.CreatedAtDate, null, true, milestone.StartDate, null, milestone.DeadlineDate, milestone.CompleteDate, null, milestone.Remarks, null, null);
            try
            {
                _dal.Task.Update(doMilestone);
            }
            catch (Exception ex)
            {
                throw new BO.BlDoesNotExistException(ex.Message, ex);
            }
        }
    }
}
