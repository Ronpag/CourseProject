using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class RegisterTaskWindow : Window
{
    public RegisterTaskWindow()
    {
        InitializeComponent();

        ClientsBox.ItemsSource = ClientService.GetAll();
        UsersBox.ItemsSource = UserService.GetUsers();
    }

    private void AssignUserChanged(object sender, RoutedEventArgs e)
    {
        UsersBox.Visibility =
            AssignUserCheckBox.IsChecked == true
                ? Visibility.Visible
                : Visibility.Collapsed;
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        string taskName = TaskNameBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(taskName))
        {
            MessageBox.Show("Task name is empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!ValidationService.ValidateEnglishText(taskName, "Task name"))
            return;

        string description = DescriptionBox.Text.Trim();
        if (!string.IsNullOrWhiteSpace(description) && !ValidationService.IsEnglishText(description))
        {
            MessageBox.Show("Description must contain only English characters", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (ClientsBox.SelectedItem is not Client selectedClient)
        {
            MessageBox.Show("Select client", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        int? UserId = null;
        var status = CRM.Data.Task.TaskStatus.Available;
        DateTime? acceptanceDate = null;

        if (AssignUserCheckBox.IsChecked == true)
        {
            if (UsersBox.SelectedItem is not User selectedUser)
            {
                MessageBox.Show("Select User", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UserId = selectedUser.Id;
            status = CRM.Data.Task.TaskStatus.Assigned;
            acceptanceDate = DateTime.Now;
        }

        if (TaskService.Create(taskName, DescriptionBox.Text.Trim(),
                selectedClient.Id, UserId, status,
                DateTime.Now, acceptanceDate))
        {
            DialogResult = true;
            Close();
        }
    }
}
