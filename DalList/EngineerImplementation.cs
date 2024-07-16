namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Adding a new object of type Engineer to a database, (to the list of objects of type Engineer).
    /// </summary>
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null)
             throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;

    }

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of objects of type Engineer.
    /// </summary>
    public void Delete(int id)
    {
        if (Read(id) is not null)
        {
            if (DataSource.Tasks.Any(task => task.EngineerId == id))
            {
                throw new DalDeletionImpossible($"A task is depends on engineer with ID={id}");
            }
            DataSource.Engineers.RemoveAll(item => item.Id == id);
        }
        else
        {
            throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
        }
    }

    /// <summary>
    /// Returning a reference to a single object of type Engineer with a certain ID, if it exists in a database (in a list of data of type Engineer), or null if the object does not exist.
    /// </summary>
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.Find(er => er.Id == id);
    }
    /// <summary>
    /// The method will allow you to select a boolean function, delegate the Func type, which will operate on one of the members of the list of type Engineer and return the first object in the list on which the function returns true.
    /// </summary>
    public Engineer Read(Func<Engineer, bool>? filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }
    /// <summary>
    /// Return a copy of the list of references to all objects of type Engineer
    /// </summary>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item);
        else
            return DataSource.Engineers.Where(filter);
    }

    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    public void Update(Engineer item)
    {
        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} doesn't exists");
        int id= item.Id;
        DataSource.Engineers.RemoveAll(item => item.Id == id);
        DataSource.Engineers.Add(item);
    }
    public void Reset()
    {
        DataSource.Engineers.Clear();
    }
}
