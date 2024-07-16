using BO;

namespace BlTest
{
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

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
                        Console.WriteLine("Enter id, name, email, a number to choose experience and cost");
                        int idEngineer, currentNum;
                        string nameEngineer, emailEngineer;
                        BO.EngineerExperience levelEngineer;
                        double costEngineer;
                        idEngineer = int.Parse(Console.ReadLine());
                        nameEngineer = Console.ReadLine();
                        emailEngineer = Console.ReadLine();
                        costEngineer = double.Parse(Console.ReadLine());
                        currentNum = int.Parse(Console.ReadLine());
                        switch (currentNum)
                        {
                            case 1: levelEngineer = BO.EngineerExperience.Novice; break;
                            case 2: levelEngineer = BO.EngineerExperience.AdvancedBeginner; break;
                            case 3: levelEngineer = BO.EngineerExperience.Competent; break;
                            case 4: levelEngineer = BO.EngineerExperience.Proficient; break;
                            case 5: levelEngineer = BO.EngineerExperience.Expert; break;
                            default: levelEngineer = BO.EngineerExperience.Expert; break;
                        }
                        BO.Engineer newEngineer = new BO.Engineer()
                        {
                            Id = idEngineer,
                            Name = nameEngineer,
                            Email = emailEngineer,
                            Level = levelEngineer,
                            Cost = costEngineer,
                            Task = null
                        };
                        s_bl.Engineer.Create(newEngineer);
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if (s_bl.Engineer!.Read(id) is null)
                            Console.WriteLine("no engineer found");
                        Console.WriteLine(s_bl.Engineer!.Read(id).ToString());
                        break;
                    case 3:
                        foreach (var engineer in s_bl.Engineer!.ReadAll())
                            Console.WriteLine(engineer.ToString());
                        break;
                    case 4:
                        int idEngineerUpdate, currentNumUpdate;
                        string nameEngineerUpdate, emailEngineerUpdate;
                        BO.EngineerExperience levelEngineerUpdate;
                        double costEngineerUpdate;
                        Console.WriteLine("Enter id for reading");
                        idEngineerUpdate = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_bl.Engineer!.Read(idEngineerUpdate).ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        nameEngineerUpdate = (Console.ReadLine());
                        emailEngineerUpdate = Console.ReadLine();
                        costEngineerUpdate = double.Parse(Console.ReadLine());
                        currentNumUpdate = int.Parse(Console.ReadLine());
                        switch (currentNumUpdate)
                        {
                            case 1: levelEngineerUpdate = BO.EngineerExperience.Novice; break;
                            case 2: levelEngineerUpdate = BO.EngineerExperience.AdvancedBeginner; break;
                            case 3: levelEngineerUpdate = BO.EngineerExperience.Competent; break;
                            case 4: levelEngineerUpdate = BO.EngineerExperience.Proficient; break;
                            case 5: levelEngineerUpdate = BO.EngineerExperience.Expert; break;
                            default: levelEngineerUpdate = BO.EngineerExperience.Expert; break;
                        }
                        BO.Engineer newEngineerUpdate = new BO.Engineer()
                        {
                            Id = idEngineerUpdate,
                            Name = nameEngineerUpdate,
                            Email = emailEngineerUpdate,
                            Level = levelEngineerUpdate,
                            Cost = costEngineerUpdate,
                            Task = null
                        };
                        s_bl.Engineer!.Update(newEngineerUpdate);
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        try
                        {
                            s_bl.Engineer!.Delete(idDelete);

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
                        int taskCopmlexityLevel = int.Parse(Console.ReadLine());
                        BO.EngineerExperience taskCopmlexity = (BO.EngineerExperience)taskCopmlexityLevel;
                        Console.Write("Current Task Number: ");

                        BO.Task newTask = new BO.Task()
                        {
                            Id = 0,
                            Alias = taskAlias,
                            Description = taskDescription,
                            CreatedAtDate = DateTime.Today,
                            Status = Status.Unscheduled,
                            Dependencies = null,
                            Milestone = null,
                            RequiredEffortTime = taskRequiredEffortTime,
                            StartDate = taskStartDate,
                            ScheduledDate = taskScheduledDate,
                            ForecastDate = null,
                            DeadlineDate = taskDeadlineDate,
                            CompleteDate = taskCompleteDate,
                            Deliverables = null,
                            Remarks = taskRemarks,
                            Engineer = null,
                            Copmlexity = taskCopmlexity
                        };
                        s_bl.Task.Create(newTask);
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if (s_bl.Task!.Read(id) is null)
                            Console.WriteLine("no task found");
                        Console.WriteLine(s_bl.Task!.Read(id).ToString());
                        break;
                    case 3:
                        foreach (var task in s_bl.Task!.ReadAll())
                            Console.WriteLine(task.ToString());
                        break;
                    case 4:

                        int currentTaskNumUpdate, idTaskUpdate;
                        string taskAliasUpdate, taskDescriptionUpdate, taskRemarksUpdate;
                        DateTime taskCreateAtDateUpdate, taskStartDateUpdate, taskScheduledDateUpdate, taskDeadlineDateUpdate, taskCompleteDateUpdate;
                        TimeSpan? taskRequiredEffortTimeUpdate;
                        BO.EngineerExperience taskCopmlexityUpdate;



                        Console.WriteLine("Enter id for reading");
                        idTaskUpdate = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_bl.Task!.Read(idTaskUpdate).ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        taskAliasUpdate = Console.ReadLine();
                        taskDescriptionUpdate = Console.ReadLine();
                        taskCreateAtDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskRequiredEffortTimeUpdate = TimeSpan.Parse(Console.ReadLine());
                        taskStartDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskScheduledDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskDeadlineDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskCompleteDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskRemarksUpdate = Console.ReadLine();
                        currentTaskNumUpdate = int.Parse(Console.ReadLine());
                        switch (currentTaskNumUpdate)
                        {
                            case 1: taskCopmlexityUpdate = BO.EngineerExperience.Novice; break;
                            case 2: taskCopmlexityUpdate = BO.EngineerExperience.AdvancedBeginner; break;
                            case 3: taskCopmlexityUpdate = BO.EngineerExperience.Competent; break;
                            case 4: taskCopmlexityUpdate = BO.EngineerExperience.Proficient; break;
                            case 5: taskCopmlexityUpdate = BO.EngineerExperience.Expert; break;
                            default: taskCopmlexityUpdate = BO.EngineerExperience.Expert; break;
                        }
                        BO.Task updateTask = new BO.Task()
                        {
                            Id = idTaskUpdate,
                            Alias = taskAliasUpdate,
                            Description = taskDescriptionUpdate,
                            CreatedAtDate = taskCreateAtDateUpdate,
                            Status = Status.Unscheduled,
                            Dependencies = null,
                            Milestone = null,
                            RequiredEffortTime = taskRequiredEffortTimeUpdate,
                            StartDate = taskStartDateUpdate,
                            ScheduledDate = taskScheduledDateUpdate,
                            ForecastDate = null,
                            DeadlineDate = taskDeadlineDateUpdate,
                            CompleteDate = taskCompleteDateUpdate,
                            Deliverables = null,
                            Remarks = taskRemarksUpdate,
                            Engineer = null,
                            Copmlexity = taskCopmlexityUpdate
                        };
                        s_bl.Task!.Update(updateTask);
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        try
                        {
                            s_bl.Task!.Delete(idDelete);
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

        //private static void MilestoneMenu()
        //{
        //    int chooseSubMenu;

        //    do
        //    {
        //        Console.WriteLine("for exit press 0\n" +
        //                  "for read an milestone press 1\n" +
        //                  "for update an dependency press 2\n");
        //        chooseSubMenu = int.Parse(Console.ReadLine());

        //        switch (chooseSubMenu)
        //        {
        //            case 1:
        //                Console.WriteLine("Enter details for all the characteristics");
        //                int dependentTask, dependsOnTask;
        //                dependentTask = int.Parse(Console.ReadLine());
        //                dependsOnTask = int.Parse(Console.ReadLine());
        //                s_dal.Dependency!.Create(new Dependency(0, dependentTask, dependsOnTask));
        //                break;
        //            case 2:
        //                int id;
        //                Console.WriteLine("Enter id for reading");
        //                id = int.Parse(Console.ReadLine());
        //                if (s_dal.Dependency!.Read(id) is null)
        //                    Console.WriteLine("no dependency found");
        //                Console.WriteLine(s_dal.Dependency!.Read(id).ToString());
        //                break;
        //            case 3:
        //                foreach (var dependency in s_dal.Dependency!.ReadAll())
        //                    Console.WriteLine(dependency.ToString());
        //                break;
        //            case 4:
        //                int idUpdate, dependentTaskUpdate, dependsOnTaskUpdate;
        //                Console.WriteLine("Enter id for reading");
        //                idUpdate = int.Parse(Console.ReadLine());
        //                Console.WriteLine(s_dal.Dependency!.Read(idUpdate).ToString());
        //                Console.WriteLine("Enter details to update");
        //                dependentTaskUpdate = int.Parse(Console.ReadLine());
        //                dependsOnTaskUpdate = int.Parse(Console.ReadLine());
        //                Dependency newDependencyUpdate = new(idUpdate, dependentTaskUpdate, dependsOnTaskUpdate);
        //                try
        //                {
        //                    s_dal.Dependency!.Update(newDependencyUpdate);

        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //                break;
        //            case 5:
        //                int idDelete;
        //                Console.WriteLine("Enter id for deleting");
        //                idDelete = int.Parse(Console.ReadLine());
        //                try
        //                {
        //                    s_dal.Dependency!.Delete(idDelete);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //                break;
        //            default: return;
        //        }

        //    } while (chooseSubMenu > 0 && chooseSubMenu < 3);
        //}
            static void Main(string[] args)
            {
            try
            {

                //Initialization.Do(s_dalDependency, s_dalEngineer, s_dalTask); //stage 1
                //Initialization.Do(s_dal); //stage 2
                Console.Write("Would you like to create Initial data? (Y/N)");
                string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                if (ans == "Y")
                    DalTest.Initialization.Do();
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
                        //case 3:

                        //    DependencyMenu();
                        //    break;
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
       
  
