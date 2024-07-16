using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        // Static reference to the business logic layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // Default engineer experience level
        public BO.EngineerExperience EngineerExperience { get; set; } = BO.EngineerExperience.None;

        /// <summary>
        /// Constructor for the EngineerListWindow.
        /// </summary>
        public EngineerListWindow()
        {
            try
            {
                InitializeComponent();
                // Populate the engineer list from the business logic layer
                var temp = s_bl?.Engineer.ReadAll();
                EngineerList = temp;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during window initialization
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Updates the engineer list after the EngineerWindow is closed.
        /// </summary>
        private void UpdateListAfterEnginnerWindowClosed()
        {
            try
            {
                // Update the engineer list from the business logic layer
                var temp = s_bl?.Engineer.ReadAll();
                EngineerList = temp;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during list update
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// The list of engineers displayed in the window.
        /// </summary>
        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        /// <summary>
        /// Dependency property for the EngineerList.
        /// </summary>
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        /// <summary>
        /// Event handler for the EngineerExperience ComboBox selection change.
        /// </summary>
        private void CBEngineerExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Update the engineer list based on the selected experience level
                EngineerList = (EngineerExperience == BO.EngineerExperience.None) ?
                    s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == EngineerExperience)!;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during list update
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Event handler for the Add Engineer button click.
        /// </summary>
        private void BtnAddEngineer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Open a new EngineerWindow for adding an engineer
                var engineerWindow = new EngineerWindow();
                engineerWindow.Closed += (sender, e) => UpdateListAfterEnginnerWindowClosed();
                engineerWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during engineer window opening
                MessageBox.Show($"{ex}", "Confirmation", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Event handler for updating an engineer from the list.
        /// </summary>
        private void UpdateEngineer_click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the selected engineer from the list
                BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;

                if (engineer != null)
                {
                    // Open an EngineerWindow for updating the selected engineer
                    var engineerWindow = new EngineerWindow(engineer.Id);
                    engineerWindow.Closed += (s, args) => UpdateListAfterEnginnerWindowClosed();
                    engineerWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during engineer update
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}

