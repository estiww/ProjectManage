using Dal;
using DalApi;
using DO;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Xml.Linq;


namespace DalTest
{
    internal class Program
    {
        //private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
        //private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        //private static ITask? s_dalTask = new TaskImplementation(); //stage 1

        //static readonly IDal s_dal = new Dal.DalList(); //stage 2
        //static readonly IDal s_dal = new DalXml(); //stage 3
        static readonly IDal s_dal = Factory.Get; //stage 4

        /// <summary>
        /// A function that allows operations to be performed on the engineer entity
        /// </summary>
        private static void EngineerMenu()
        {
            int chooseSubMenu;
            do
            {
                Console.WriteLine("for exit press 0\n" +
                          "for add an engineer press 1\n" +
                          "for read an engineer press 2\n" +
                          "for read all engineers press 3\n" +
                          "for update an engineer press 4\n" +
                          "for delete an engineer press 5\n");
                chooseSubMenu = int.Parse(Console.ReadLine());

                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter id, name, email, cost and a number to choose experience");
                        int idEngineer, currentNum;
                        string nameEngineer, emailEngineer;
                        EngineerExperience levelEngineer;
                        double costEngineer;
                        idEngineer = int.Parse(Console.ReadLine());
                        nameEngineer = Console.ReadLine();
                        emailEngineer = Console.ReadLine();
                        costEngineer = double.Parse(Console.ReadLine());
                        currentNum = int.Parse(Console.ReadLine());
                        switch (currentNum)
                        {
                            case 1: levelEngineer = EngineerExperience.Novice; break;
                            case 2: levelEngineer = EngineerExperience.AdvancedBeginner; break;
                            case 3: levelEngineer = EngineerExperience.Competent; break;
                            case 4: levelEngineer = EngineerExperience.Proficient; break;
                            case 5: levelEngineer = EngineerExperience.Expert; break;
                            default: levelEngineer = EngineerExperience.Expert; break;
                        }
                        s_dal.Engineer.Create(new Engineer(idEngineer, nameEngineer, emailEngineer, levelEngineer, costEngineer));
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if (s_dal.Engineer!.Read(id) is null)
                            Console.WriteLine("no engineer found");
                        Console.WriteLine(s_dal.Engineer!.Read(id).ToString());
                        break;
                    case 3:
                        foreach (var engineer in s_dal.Engineer!.ReadAll())
                            Console.WriteLine(engineer.ToString());
                        break;
                    case 4:
                        int idEngineerUpdate, currentNumUpdate;
                        string nameEngineerUpdate, emailEngineerUpdate;
                        EngineerExperience levelEngineerUpdate;
                        double costEngineerUpdate;
                        Console.WriteLine("Enter id for reading");
                        idEngineerUpdate = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal.Engineer!.Read(idEngineerUpdate).ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        nameEngineerUpdate = (Console.ReadLine());
                        emailEngineerUpdate = Console.ReadLine();
                        costEngineerUpdate = double.Parse(Console.ReadLine());
                        currentNumUpdate = int.Parse(Console.ReadLine());
                        switch (currentNumUpdate)
                        {
                            case 1: levelEngineerUpdate = EngineerExperience.Novice; break;
                            case 2: levelEngineerUpdate = EngineerExperience.AdvancedBeginner; break;
                            case 3: levelEngineerUpdate = EngineerExperience.Competent; break;
                            case 4: levelEngineerUpdate = EngineerExperience.Proficient; break;
                            case 5: levelEngineerUpdate = EngineerExperience.Expert; break;
                            default: levelEngineerUpdate = EngineerExperience.Expert; break;
                        }
                        Engineer newEngineerUpdate = new(idEngineerUpdate, nameEngineerUpdate, emailEngineerUpdate, levelEngineerUpdate, costEngineerUpdate);
                        s_dal.Engineer!.Update(newEngineerUpdate);
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        try
                        {
                            s_dal.Engineer!.Delete(idDelete);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        /// <summary>
        /// A function that allows operations to be performed on the dependency entity
        /// </summary>
        private static void DependencyMenu()
        {
            int chooseSubMenu;

            do
            {
                Console.WriteLine("for exit press 0\n" +
                          "for add an dependency press 1\n" +
                          "for read an dependency press 2\n" +
                          "for read all dependencies press 3\n" +
                          "for update an dependency press 4\n" +
                          "for delete an dependency press 5\n");
                chooseSubMenu = int.Parse(Console.ReadLine());

                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter details for all the characteristics");
                        int dependentTask, dependsOnTask;
                        dependentTask = int.Parse(Console.ReadLine());
                        dependsOnTask = int.Parse(Console.ReadLine());
                        s_dal.Dependency!.Create(new Dependency(0, dependentTask, dependsOnTask));
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if (s_dal.Dependency!.Read(id) is null)
                            Console.WriteLine("no dependency found");
                        Console.WriteLine(s_dal.Dependency!.Read(id).ToString());
                        break;
                    case 3:
                        foreach (var dependency in s_dal.Dependency!.ReadAll())
                            Console.WriteLine(dependency.ToString());
                        break;
                    case 4:
                        int idUpdate, dependentTaskUpdate, dependsOnTaskUpdate;
                        Console.WriteLine("Enter id for reading");
                        idUpdate = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal.Dependency!.Read(idUpdate).ToString());
                        Console.WriteLine("Enter details to update");
                        dependentTaskUpdate = int.Parse(Console.ReadLine());
                        dependsOnTaskUpdate = int.Parse(Console.ReadLine());
                        Dependency newDependencyUpdate = new(idUpdate, dependentTaskUpdate, dependsOnTaskUpdate);
                        try
                        {
                            s_dal.Dependency!.Update(newDependencyUpdate);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        try
                        {
                            s_dal.Dependency!.Delete(idDelete);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }
        /// <summary>
        /// A function that allows operations to be performed on the task entity
        /// </summary>
        private static void TaskMenu()
        {
            int chooseSubMenu;
            do
            {
                Console.WriteLine("for exit press 0\n" +
                          "for add an task press 1\n" +
                          "for read an task press 2\n" +
                          "for read all tasks press 3\n" +
                          "for update an task press 4\n" +
                          "for delete an task press 5\n");
                chooseSubMenu = int.Parse(Console.ReadLine());
                switch (chooseSubMenu)
                {
  
                    case 1:
                        Console.WriteLine("Enter task information:");

                        Console.Write("Alias: ");
                        string taskAlias = Console.ReadLine();

                        Console.Write("Description: ");
                        string taskDescription = Console.ReadLine();

                        Console.Write("CreatedAtDate: ");
                        DateTime taskCreatedAtDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("RequiredEffortTime (HH:mm): ");
                        TimeSpan? taskRequiredEffortTime = TimeSpan.Parse(Console.ReadLine());

                        Console.Write("IsMilestone (true/false): ");
                        bool taskIsMilestone = bool.Parse(Console.ReadLine());

                        Console.Write("StartDate: ");
                        DateTime? taskStartDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("ScheduledDate: ");
                        DateTime? taskScheduledDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("DeadlineDate: ");
                        DateTime? taskDeadlineDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("CompleteDate: ");
                        DateTime? taskCompleteDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("Deriverables: ");
                        string taskDeriverables = Console.ReadLine();

                        Console.Write("Remarks: ");
                        string taskRemarks = Console.ReadLine();

                        Console.Write("EngineerId: ");
                        int taskEngineerId = int.Parse(Console.ReadLine());

                        Console.Write("Task's Level (0 for Easy, 1 for Moderate, 2 for Complex): ");
                        int taskComplexityLevel = int.Parse(Console.ReadLine());
                        EngineerExperience taskComplexity = (EngineerExperience)taskComplexityLevel;

                        Console.Write("Current Task Number: ");
                        s_dal.Task.Create(new DO.Task(0, taskAlias,taskDescription, taskCreatedAtDate, taskRequiredEffortTime,taskIsMilestone,  taskStartDate, taskScheduledDate, taskDeadlineDate, taskCompleteDate, taskDeriverables, taskRemarks, taskEngineerId, taskComplexity));
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if (s_dal.Task!.Read(id) is null)
                            Console.WriteLine("no task found");
                        Console.WriteLine(s_dal.Task!.Read(id).ToString());
                        break;
                    case 3:
                        foreach (var task in s_dal.Task!.ReadAll())
                            Console.WriteLine(task.ToString());
                        break;
                    case 4:
                        //                 int Id,
                        //string Alias,
                        //string Description,
                        //DateTime CreatedAtDate,
                        //TimeSpan? RequiredEffortTime,
                        //bool IsMilestone,
                        //DateTime? StartDate,  
                        //DateTime? ScheduledDate,
                        //DateTime? DeadLineDate,
                        //DateTime? CompleteDate,
                        //string? Deriverables,
                        //string? Remarks,
                        //int? EngineerId,
                        //EngineerExperience Copmlexity

                        //int taskEngineerId, currentTaskNum;
                        //string taskAlias, taskDescription, taskDeliverables, taskRemarks;
                        //bool taskIsMilestone;
                        //DateTime taskCreateAtDate, taskStartDate, taskScheduledDate, taskDeadlineDate, taskCompleteDate;
                        //TimeSpan taskRequiredEffortTime;
                        //EngineerExperience taskCopmlexity;
                        int taskEngineerIdUpdate, currentTaskNumUpdate,idTaskUpdate ;
                        string taskAliasUpdate,taskDescriptionUpdate,  taskDeliverablesUpdate, taskRemarksUpdate;
                        bool taskIsMilestoneUpdate;
                        DateTime taskCreateAtDateUpdate, taskStartDateUpdate, taskScheduledDateUpdate, taskDeadlineDateUpdate, taskCompleteDateUpdate;
                        TimeSpan? taskRequiredEffortTimeUpdate;
                        EngineerExperience taskCopmlexityUpdate;



                        Console.WriteLine("Enter id for reading");
                        idTaskUpdate = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal.Task!.Read(idTaskUpdate).ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        taskAliasUpdate = Console.ReadLine(); 
                        taskDescriptionUpdate = Console.ReadLine();
                        taskCreateAtDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskRequiredEffortTimeUpdate = TimeSpan.Parse(Console.ReadLine());
                        taskIsMilestoneUpdate = bool.Parse(Console.ReadLine());
                        taskStartDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskScheduledDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskDeadlineDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskCompleteDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskDeliverablesUpdate = Console.ReadLine();
                        taskRemarksUpdate = Console.ReadLine();
                        taskEngineerIdUpdate = int.Parse(Console.ReadLine());
                        currentTaskNumUpdate = int.Parse(Console.ReadLine());
                        switch (currentTaskNumUpdate)
                        {
                            case 1: taskCopmlexityUpdate = EngineerExperience.Novice; break;
                            case 2: taskCopmlexityUpdate = EngineerExperience.AdvancedBeginner; break;
                            case 3: taskCopmlexityUpdate = EngineerExperience.Competent; break;
                            case 4: taskCopmlexityUpdate = EngineerExperience.Proficient; break;
                            case 5: taskCopmlexityUpdate = EngineerExperience.Expert; break;
                            default: taskCopmlexityUpdate = EngineerExperience.Expert; break;
                        }
                        DO.Task newTaskUpdate = new(idTaskUpdate,taskAliasUpdate, taskDescriptionUpdate, taskCreateAtDateUpdate, taskRequiredEffortTimeUpdate, taskIsMilestoneUpdate, taskStartDateUpdate, taskScheduledDateUpdate, taskDeadlineDateUpdate, taskCompleteDateUpdate, taskDeliverablesUpdate, taskRemarksUpdate, taskEngineerIdUpdate, taskCopmlexityUpdate);
                        s_dal.Task!.Update(newTaskUpdate);
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        try
                        {
                            s_dal.Task!.Delete(idDelete);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        /// <summary>
        /// Main plan for testing a data layer
        /// The main program calls a function depending on the selected entity
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                
                //Initialization.Do(s_dalDependency, s_dalEngineer, s_dalTask); //stage 1
                //Initialization.Do(s_dal); //stage 2
                Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                if (ans == "Y") //stage 3
                    //Initialization.Do(s_dal); //stage 2
                    Initialization.Do(); //stage 4
                int chooseEntity;
                do
                {
                    Console.WriteLine("for task press 1\nfor engineer press 2\nfor dependency press 3\nfor exit press 0\n");
                    chooseEntity = int.Parse(Console.ReadLine());
                    switch (chooseEntity)
                    {
                        case 1:
                            TaskMenu();
                            break;
                        case 2:

                            EngineerMenu();
                            break;
                        case 3:

                            DependencyMenu();
                            break;
                        default:
                            break;
                    }
                }
                while (chooseEntity != 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
