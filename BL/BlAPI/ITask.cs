
namespace BlApi;

public interface ITask
{
    int Create(BO.Task item); //Creates new entity object in DAL
    BO.Task? Read(int id); //Reads entity object by its ID 
    BO.Task? Read(Func<BO.Task, bool> filter); // stage 2

    //List<T> ReadAll(); //stage 1 only, Reads all entity objects
    IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null); // stage 2
    void Update(BO.Task item); //Updates entity object
    void Delete(int id);//Deletes an object by its Id
}
