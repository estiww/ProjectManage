

namespace BlApi;

public interface IEngineer
{
    int Create(BO.Engineer item); //Creates new entity object in DAL
    BO.Engineer? Read(int id); //Reads entity object by its ID 
    BO.Engineer? Read(Func<BO.Engineer, bool> filter); // stage 2

    //List<T> ReadAll(); //stage 1 only, Reads all entity objects
    IEnumerable<BO.Engineer?> ReadAll(Func<BO.Engineer, bool>? filter = null); // stage 2
    void Update(BO.Engineer item); //Updates entity object
    void Delete(int id);//Deletes an object by its Id
}

