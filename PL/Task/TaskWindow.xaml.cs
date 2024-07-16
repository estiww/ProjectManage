using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        // Static reference to the business logic layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // Property representing the status of the task window
        public BO.Status Status { get; set; } = BO.Status.Unscheduled;

        /// <summary>
        /// The task being manipulated by the window.
        /// </summary>
        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        /// <summary>
        /// Constructor for the TaskWindow.
        /// </summary>
        /// <param name="Id">The ID of the task to load. If 0, a new task is created.</param>
        public TaskWindow(int Id = 0)
        {
            try
            {
                InitializeComponent();
                if (Id == 0)
                {
                    // Create a new task and initialize its associated engineer
                    Task = new BO.Task();
                    Task.Engineer = new BO.EngineerInTask();
                }
                else
                {
                    try
                    {
                        // Attempt to read the task with the provided ID
                        Task = s_bl.Task.Read(Id)!; // '!' used for null suppression operator
                        if (Task.Engineer == null)
                        {
                            // If the task's engineer is null, initialize it
                            Task.Engineer = new BO.EngineerInTask();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur during task retrieval
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during window initialization
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Dependency property for the Task object.
        /// </summary>
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        /// <summary>
        /// Event handler for the Save Task button click.
        /// </summary>
        private void BtnSaveTask_Click(object sender, RoutedEventArgs e)
        {
            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(Task.Alias) || string.IsNullOrWhiteSpace(Task.Description))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if Complexity is selected
            if (Task.Copmlexity == 0)
            {
                MessageBox.Show("Please select a Complexity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Determine the action based on the clicked button content
            Button clickedButton = (Button)sender;
            object contentValue = clickedButton.Content;
            if (contentValue == "Add")
            {
                try
                {
                    // Create a new task
                    s_bl.Task.Create(Task);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during task creation
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                // Update an existing task
                if (Task.Engineer?.Id == 0)
                {
                    // If engineer ID is 0, set engineer to null
                    Task.Engineer = null;
                }

                // Validate and update task
                if (((Task.Engineer) != null))
                {
                    try
                    {
                        // Attempt to read the engineer associated with the task
                        s_bl.Engineer.Read(Task.Engineer.Id);
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur during engineer retrieval
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                try
                {
                    // Update the task
                    s_bl.Task.Update(Task);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during task update
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            // Close the window after the operation is completed
            this.Close();
        }

        private void BtnAddDependency_Click(object sender, RoutedEventArgs e)
        {
            if (addDependency.Text == Task.Id.ToString())
            {
                MessageBox.Show("can not create a task dependency for itself", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (var dependency in Task.Dependencies)
            {
                if (dependency.Id.ToString() == addDependency.Text)
                {
                    MessageBox.Show("A task dependency with the same ID already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            try
            {
                s_bl.Task.Read(Convert.ToInt32(addDependency.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            var task = s_bl.Task.Read(Convert.ToInt32(addDependency.Text));
            var taskInList = new BO.TaskInList
            {
                Id = task.Id,
                Description = task.Description,
                Alias = task.Alias,
                Status = task.Status,
            };
            
            Task.Dependencies.Add(taskInList);
            

        }
    }
}
