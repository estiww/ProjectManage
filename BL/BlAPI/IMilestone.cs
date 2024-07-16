
using BO;

namespace BlApi;

public interface IMilestone
{
    void Create(); //Creates new entity object in DAL
    BO.Milestone? Read(int id); //Reads entity object by its ID 
    void Update(BO.Milestone item); //Updates entity object
}
