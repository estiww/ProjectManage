using DalApi;
using System.Reflection;

namespace BO;

internal static class Tools

{ /// <summary>
  /// Generates a string representation of an object.
  /// </summary>
  /// <param name="p">The object to generate the string representation for.</param>
  /// <returns>A string representation of the object.</returns>
    public static string GenericToString(this object p)
    {
        var prop = p.GetType().GetProperties();
        string str = "";
        foreach (var property in prop)
        {
            if (property.Name == "Dependencies")
            {
                var dependenciesValue = property.GetValue(p);
                if (dependenciesValue != null)
                {
                    var dependenciesList = (System.Collections.IEnumerable)dependenciesValue;
                    str += $"{property.Name}: [";

                    foreach (var taskInList in dependenciesList)
                    {
                        var taskProperties = taskInList.GetType().GetProperties();
                        str += "{ ";

                        foreach (var taskProperty in taskProperties)
                        {
                            str += $"{taskProperty.Name}: {taskProperty.GetValue(taskInList)}, ";
                        }

                        str = str.TrimEnd(',', ' ') + " }, ";
                    }

                    str = str.TrimEnd(',', ' ') + "]";
                }
            }
            else
                    {
                str += $" {property.Name}: {property.GetValue(p)},";
            }
        }

        return str.TrimEnd(',', ' ');
    }
}



