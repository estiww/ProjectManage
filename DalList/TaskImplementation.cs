namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;




internal class TaskImplementation : ITask
{
    /// <summary>
    /// Adding a new object of type Task to a database, (to the list of objects of type Task).
    /// </summary>
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;

    }

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of objects of type Task.
    /// </summary>

    public void Delete(int id)
    {
        if (Read(id) is not null)
        {
            if (DataSource.Dependencies.Any(dependency => dependency.DependsOnTask == id))
            {
                throw new DalDeletionImpossible($"Another task depends on task with ID={id}");
            }

            DataSource.Tasks.RemoveAll(item => item.Id == id);

            DataSource.Dependencies.RemoveAll(dependency => dependency.DependentTask == id);
        }
        else 
        { 
           throw new DalDoesNotExistException($"Task with ID={id} doesn't exists");
        }
    }
    /// <summary>
    /// Returning a reference to a single object of type Task with a certain ID, if it exists in a database (in a list of data of type Task), or null if the object does not exist.
    /// </summary>
    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(tk => tk.Id == id);
    }
    /// <summary>
    /// The method will allow you to select a boolean function, delegate the Func type, which will operate on one of the members of the list of type Task and return the first object in the list on which the function returns true.
    /// </summary>
    public Task Read(Func<Task, bool>? filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }
    /// <summary>
    /// Return a copy of the list of references to all objects of type Task
    /// </summary>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);
    }

    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    public void Update(Task item)
    {
        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} doesn't exists");
        int id = item.Id;
        DataSource.Tasks.RemoveAll(item => item.Id==id);
        DataSource.Tasks.Add(item);
    }

    public void Reset()
    {
        DataSource.Tasks.Clear();
    }
}
