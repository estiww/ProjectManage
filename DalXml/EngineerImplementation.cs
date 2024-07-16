namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;

internal class EngineerImplementation : IEngineer
{
    const string engineersFile = @"engineers";
    const string tasksFile = @"tasks";

    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(engineersFile);
        if (Read(item.Id) is not null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, engineersFile);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(engineersFile);

        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(tasksFile);


        if (Read(id) is not null)
        {
            if (tasks.Any(task => task.EngineerId == id))
            {
                throw new DalDeletionImpossible($"A task is depends on engineer with ID={id}");
            }
            engineers.RemoveAll(item => item.Id == id);
        }
        else
        {
            throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
        }
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, engineersFile);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks,tasksFile );


    }

    public Engineer? Read(int id)
    {
       
        return XMLTools.LoadListFromXMLSerializer<Engineer>(engineersFile).Find(er => er.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {

            if (filter == null)
                return (Engineer?)XMLTools.LoadListFromXMLSerializer<Engineer>(engineersFile).Select(item => item);
            else
                return (Engineer?)XMLTools.LoadListFromXMLSerializer<Engineer>(engineersFile).Where(filter);
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        return XMLTools.LoadListFromXMLSerializer<Engineer>(engineersFile);
    }

    public void Reset()
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(engineersFile);
        engineers.Clear();
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, engineersFile);
    }

    public void Update(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(engineersFile);
        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} doesn't exists");
        int id = item.Id;
        engineers.RemoveAll(item => item.Id == id);
        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, engineersFile);
    }
}
