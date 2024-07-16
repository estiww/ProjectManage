using BlApi;
using BO;

namespace BlImplementation
{
    /// <summary>
    /// Implementation of the <see cref="IEngineer"/> interface.
    /// </summary>
    internal class EngineerImplementation : IEngineer
    {
        private readonly DalApi.IDal _dal = DalApi.Factory.Get;

        /// <summary>
        /// Creates a new engineer in the system.
        /// </summary>
        /// <param name="boEngineer">The details of the new engineer to create.</param>
        /// <returns>The unique identifier of the newly created engineer.</returns>
        public int Create(BO.Engineer boEngineer)
        {
            // Create a new DO.Engineer object from the provided BO.Engineer object.
            DO.Engineer doEngineer = new DO.Engineer(
                boEngineer.Id,
                boEngineer.Name,
                boEngineer.Email,
                (DO.EngineerExperience)boEngineer.Level,
                boEngineer.Cost);

            try
            {
                // Call the DAL to create the engineer in the data store.
                int idEngineer = _dal.Engineer.Create(doEngineer);
                return idEngineer;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                // If the engineer already exists, throw a BL exception.
                throw new BO.BlAlreadyExistsException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Deletes an engineer from the system based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the engineer to delete.</param>
        public void Delete(int id)
        {
            try
            {
                // Call the DAL to delete the engineer from the data store.
                _dal.Engineer.Delete(id);
            }
            catch (DO.DalDeletionImpossible ex)
            {
                // If deletion is impossible, throw a BL exception.
                throw new BO.BlDeletionImpossible(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads details of an engineer from the system based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the engineer to read.</param>
        /// <returns>The details of the engineer.</returns>
        public BO.Engineer? Read(int id)
        {
            // Call the DAL to read the engineer from the data store.
            DO.Engineer? doEngineer = _dal.Engineer.Read(id);

            // If the engineer doesn't exist, throw a BL exception.
            return doEngineer == null
                ? throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist")
                : new BO.Engineer()
                {
                    Id = id,
                    Name = doEngineer.Name,
                    Email = doEngineer.Email,
                    Level = (BO.EngineerExperience)doEngineer.Level,
                    Cost = doEngineer.Cost,
                    Task = ReadTaskInEngineer(doEngineer.Id),
                };
        }

        public BO.Engineer? Read(Func<BO.Engineer, bool> filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads details of all engineers from the system, optionally applying a filter.
        /// </summary>
        public IEnumerable<BO.Engineer?> ReadAll(Func<BO.Engineer, bool>? filter = null)
        {
            Func<BO.Engineer, bool> filter1 = filter != null ? filter! : item => true;
            return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                    select new BO.Engineer()
                    {
                        Id = doEngineer.Id,
                        Name = doEngineer.Name,
                        Email = doEngineer.Email,
                        Level = (BO.EngineerExperience)doEngineer.Level,
                        Cost = doEngineer.Cost,
                        Task = ReadTaskInEngineer(doEngineer.Id),
                    }).Where(filter1);
        }

        /// <summary>
        /// Updates details of an engineer in the system.
        /// </summary>
        /// <param name="boEngineer">The updated details of the engineer.</param>
        public void Update(BO.Engineer boEngineer)
        {
            // Check if the engineer exists.
            if (Read(boEngineer.Id) is null)
                throw new BO.BlDoesNotExistException($"Task with ID={boEngineer.Id} does Not exist");

            // Create a new DO.Engineer object from the provided BO.Engineer object.
            DO.Engineer doEngineer = new DO.Engineer(
                boEngineer.Id,
                boEngineer.Name,
                boEngineer.Email,
                (DO.EngineerExperience)boEngineer.Level,
                boEngineer.Cost);

            try
            {
                // Call the DAL to update the engineer in the data store.
                _dal.Engineer.Update(doEngineer);
            }
            catch (Exception ex)
            {
                // If an exception occurred during the update process, throw a BL exception.
                throw new BO.BlDoesNotExistException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads a task assigned to the engineer with the provided ID.
        /// </summary>
        /// <param name="id">The ID of the engineer.</param>
        /// <returns>The task assigned to the engineer.</returns>
        public TaskInEngineer? ReadTaskInEngineer(int id)
        {
            // If the provided ID is null, return null.
            if (id == null)
                return null;

            // Find the task assigned to the engineer with the provided ID.
            var doTask = _dal.Task.ReadAll()
                .FirstOrDefault(doTask => doTask.EngineerId == id);

            // If no task is found, return null.
            if (doTask == null)
                return null;

            // Create a new TaskInEngineer object with details of the found task.
            return new TaskInEngineer
            {
                Id = doTask.Id,
                Alias = doTask.Alias
            };
        }
    }
}
