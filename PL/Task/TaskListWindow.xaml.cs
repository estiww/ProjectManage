using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        // Static reference to the business logic layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // Default engineer experience level
        public BO.EngineerExperience EngineerExperience { get; set; } = BO.EngineerExperience.None;

        /// <summary>
        /// Constructor for the TaskListWindow.
        /// </summary>
        public TaskListWindow()
        {
            try
            {
                InitializeComponent();
                // Populate the task list from the business logic layer
                var temp = s_bl?.Task.ReadAll()!;
                TaskList = temp;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during window initialization
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Updates the task list after the TaskWindow is closed.
        /// </summary>
        private void UpdateListAfterTaskWindowClosed()
        {
            try
            {
                // Update the task list from the business logic layer
                var temp = s_bl?.Task.ReadAll();
                TaskList = temp;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during list update
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// The list of tasks displayed in the window.
        /// </summary>
        public IEnumerable<BO.Task> TaskList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        /// <summary>
        /// Dependency property for the TaskList.
        /// </summary>
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(TaskListWindow), new PropertyMetadata(null));

        /// <summary>
        /// Event handler for the Add Task button click.
        /// </summary>
        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open a new TaskWindow for adding a task
                var taskWindow = new TaskWindow();
                taskWindow.Closed += (sender, e) => UpdateListAfterTaskWindowClosed();
                taskWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during task window opening
                MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Event handler for updating a task from the list.
        /// </summary>
        private void UpdateTask_click(object sender, RoutedEventArgs e)
        {
            // Get the selected task from the list
            BO.Task? task = (sender as ListView)?.SelectedItem as BO.Task;
            // Open a TaskWindow for updating the selected task
            if (task != null)
            {
                var taskWindow = new TaskWindow(task.Id);
                taskWindow.Closed += (s, args) => UpdateListAfterTaskWindowClosed();
                taskWindow.ShowDialog();
            }
           
        }

        /// <summary>
        /// Event handler for the Complexity ComboBox selection change.
        /// </summary>
        private void CBCopmlexity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Update the task list based on the selected complexity level
                TaskList = (EngineerExperience == BO.EngineerExperience.None) ?
                    s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Copmlexity == EngineerExperience)!;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during list update
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
