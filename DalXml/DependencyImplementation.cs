namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

internal class DependencyImplementation : IDependency      
{
    const string dependenciesFile = @"dependencies";
    const string data_config_xml = @"data-config";


    public int Create(Dependency item)
    {
        int id = Config.NextDependencyId;
        XElement dependenciesElement = XMLTools.LoadListFromXMLElement(dependenciesFile);

        XElement newDependencyElement = new XElement("Dependency",
             new XElement("Id", id),
             new XElement("DependentTask", item.DependentTask),
             new XElement("DependsOnTask", item.DependsOnTask)
         );

        dependenciesElement.Add(newDependencyElement);
        XMLTools.SaveListToXMLElement(dependenciesElement, dependenciesFile);
        return id;
    }

    public void Delete(int id)
    {
        XElement dependenciesElement = XMLTools.LoadListFromXMLElement(dependenciesFile);

        if (dependenciesElement != null)
        {
            XElement dependencyElement = dependenciesElement.Elements("Dependency")
                .FirstOrDefault(e => (int)e.Element("Id") == id);

            if (dependencyElement != null)
            {
                dependencyElement.Remove();
                XMLTools.SaveListToXMLElement(dependenciesElement, dependenciesFile);
            }
            else
            {
                throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");
            }
        }
        else
        {
            throw new DalDoesNotExistException("Dependencies document is empty.");
        }
    }

    public Dependency? Read(int id)
    {
        XElement dependenciesElement = XMLTools.LoadListFromXMLElement(dependenciesFile);
        XElement dependencyElement = dependenciesElement.Elements("Dependency").FirstOrDefault(d => (int)d.Element("Id") == id)!;
        Dependency dependency= new Dependency(
                (int)dependencyElement.Element("Id")!,
                (int)dependencyElement.Element("DependentTask")!,
                (int)dependencyElement.Element("DependsOnTask")!
            );
        return dependency;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement dependenciesElement = XMLTools.LoadListFromXMLElement(dependenciesFile);

        Dependency? dependency = dependenciesElement
        .Elements("Dependency")
        .Select(dependency => new Dependency(
            (int)dependency.Element("Id")!,
            (int)dependency.Element("DependentTask")!,
            (int)dependency.Element("DependsOnTask")!
        ))
        !.FirstOrDefault(filter);

        return dependency;
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement rootElement = XMLTools.LoadListFromXMLElement(dependenciesFile);

        var query = from depElement in rootElement.Elements("Dependency")
                    let dependency = new Dependency
                    {
                        Id = (int)depElement.Element("Id")!,
                        DependentTask = (int)depElement.Element("DependentTask")!,
                        DependsOnTask = (int)depElement.Element("DependsOnTask")!
                    }
                    where filter == null || filter(dependency)
                    select dependency;

        return query.ToList();
    }

    public void Reset()
    {
        XElement root = XMLTools.LoadListFromXMLElement(data_config_xml);
        root.Element("NextDependencyId")?.SetValue((1).ToString());
        XMLTools.SaveListToXMLElement(root, data_config_xml);
        XElement dependenciesElement = XMLTools.LoadListFromXMLElement(dependenciesFile);
        dependenciesElement.RemoveAll();
        XMLTools.SaveListToXMLElement(dependenciesElement, dependenciesFile);
       
    }

    public void Update(Dependency item)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(dependenciesFile);
        if (Read(item.Id) is null)
            throw new DalDoesNotExistException($"dependenciesFile with ID={item.Id} doesn't exists");
        int id = item.Id;
        dependencies.RemoveAll(item => item.Id == id);
        dependencies.Add(item);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, dependenciesFile);
    }
}
