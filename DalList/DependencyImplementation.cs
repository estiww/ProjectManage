namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// Adding a new object of type Dependency to a database, (to the list of objects of type Dependency).
    /// </summary>
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencies.Add(copy);
        return id;
    }

    /// <summary>
    /// Deletion of an existing object with a certain ID, from the list of objects of type Dependency.
    /// </summary>
    public void Delete(int id)
    {
        if (Read(id) is not null)
        {
            DataSource.Dependencies.RemoveAll(item => item.Id == id);
        }
        else
        {
            throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");
        }
    }

    /// <summary>
    /// Returning a reference to a single object of type Dependency with a certain ID, if it exists in a database (in a list of data of type Dependency), or null if the object does not exist.
    /// </summary>
    
    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.Find(dy => dy.Id == id);
    }
    /// <summary>
    /// The method will allow you to select a boolean function, delegate the Func type, which will operate on one of the members of the list of type Dependency and return the first object in the list on which the function returns true.
    /// </summary>
    public Dependency Read(Func<Dependency, bool>? filter)
    {
        return DataSource.Dependencies.FirstOrDefault(filter);
    }

    /// <summary>
    /// Return a copy of the list of references to all objects of type Dependency
    /// </summary>

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);

    }

    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    public void Update(Dependency item)
    {
        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} doesn't exists");
        Delete(item.Id);
        DataSource.Dependencies.Add(item);
    }
    public void Reset()
    {
        DataSource.Dependencies.Clear();
    }
}
