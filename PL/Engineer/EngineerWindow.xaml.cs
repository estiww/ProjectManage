using System;
using System.Windows;
using System.Windows.Controls;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        // Static reference to the business logic layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // Default engineer experience level
        public BO.EngineerExperience EngineerExperience { get; set; } = BO.EngineerExperience.Novice;

        /// <summary>
        /// The engineer being manipulated by the window.
        /// </summary>
        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        /// <summary>
        /// Constructor for the EngineerWindow.
        /// </summary>
        /// <param name="Id">The ID of the engineer to load. If 0, a new engineer is created.</param>
        public EngineerWindow(int Id = 0)
        {
            try
            {
                InitializeComponent();
                if (Id == 0)
                {
                    // Create a new engineer
                    Engineer = new BO.Engineer();
                }
                else
                {

                    // Attempt to read the engineer with the provided ID
                    Engineer = s_bl.Engineer.Read(Id)!; // '!' used for null suppression operator

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
        /// Dependency property for the Engineer object.
        /// </summary>
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        /// <summary>
        /// Event handler for the Save Engineer button click.
        /// </summary>
        private void BtnSaveEngineer_Click(object sender, RoutedEventArgs e)
        {
            // Check if required fields are filled and valid
            if (Engineer.Id == 0 || string.IsNullOrWhiteSpace(Engineer.Name) || string.IsNullOrWhiteSpace(Engineer.Email))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!IsValidEmail(Engineer.Email))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Engineer.Level == 0)
            {
                MessageBox.Show("Please select a level.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Determine the action based on the clicked button content
            Button clickedButton = (Button)sender;
            object contentValue = clickedButton.Content;
            if (contentValue == "Add")
            {
                try
                {
                    // Create a new engineer
                    s_bl.Engineer.Create(Engineer);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during engineer creation
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                try
                {
                    // Update an existing engineer
                    s_bl.Engineer.Update(Engineer);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during engineer update
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Close the window after the operation is completed
            this.Close();
        }

        /// <summary>
        /// Validates if the provided email address is in a valid format.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>True if the email address is valid; otherwise, false.</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}