namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Linq;



internal class TaskImplementation : ITask
{
    const string tasksFile = @"tasks";
    const string dependenciesFile = @"dependencies";
    const string data_config_xml = @"data-config";

    public int Create(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(tasksFile);
        int id = Config.NextTaskId;
        Task copy = item with { Id = id };
        tasks.Add(copy);
        XMLTools.SaveListToXMLSerializer<Task>(tasks, tasksFile);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(tasksFile);
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(dependenciesFile);

        if (Read(id) is not null)
        {
            if (dependencies.Any(dependency => dependency.DependsOnTask == id))
            {
                throw new DalDeletionImpossible($"Another task depends on task with ID={id}");
            }

            tasks.RemoveAll(item => item.Id == id);

            dependencies.RemoveAll(dependency => dependency.DependentTask == id);
        }
        else
        {
            throw new DalDoesNotExistException($"Task with ID={id} doesn't exists");
        }
        XMLTools.SaveListToXMLSerializer<Task>(tasks, tasksFile);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, dependenciesFile);


    }

    public Task? Read(int id)
    {
       
        return XMLTools.LoadListFromXMLSerializer<Task>(tasksFile).Find(tk => tk.Id == id);

    }

    public Task? Read(Func<Task, bool> filter)
    {
        if (filter == null)
            return (Task?)XMLTools.LoadListFromXMLSerializer<Task>(tasksFile).Select(item => item);
        else
            return (Task?)XMLTools.LoadListFromXMLSerializer<Task>(tasksFile).Where(filter);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        return XMLTools.LoadListFromXMLSerializer<Task>(tasksFile);
    }

    public void Reset()
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config_xml);
        root.Element("NextTaskId")?.SetValue((1).ToString());
        XMLTools.SaveListToXMLElement(root, data_config_xml);
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(tasksFile);
        tasks.Clear();
        XMLTools.SaveListToXMLSerializer<Task>(tasks, tasksFile);
      
    }

    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(tasksFile);
        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} doesn't exists");
        int id = item.Id;
        tasks.RemoveAll(item => item.Id == id);
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer<Task>(tasks, tasksFile);
    }
}
