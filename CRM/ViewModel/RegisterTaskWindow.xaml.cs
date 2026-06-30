using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class RegisterTaskWindow : Window
{
    public RegisterTaskWindow()
    {
        InitializeComponent();

        ClientsBox.ItemsSource = ClientService.GetAll();
        WorkersBox.ItemsSource = UserService.GetWorkers();
    }

    private void AssignWorkerChanged(object sender, RoutedEventArgs e)
    {
        WorkersBox.Visibility =
            AssignWorkerCheckBox.IsChecked == true
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

        if (ClientsBox.SelectedItem is not Client selectedClient)
        {
            MessageBox.Show("Select client", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        int? workerId = null;
        var status = CRM.Data.Task.TaskStatus.Available;
        DateTime? acceptanceDate = null;

        if (AssignWorkerCheckBox.IsChecked == true)
        {
            if (WorkersBox.SelectedItem is not User selectedWorker)
            {
                MessageBox.Show("Select worker", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            workerId = selectedWorker.Id;
            status = CRM.Data.Task.TaskStatus.Assigned;
            acceptanceDate = DateTime.Now;
        }

        if (TaskService.Create(taskName, DescriptionBox.Text.Trim(),
                selectedClient.Id, workerId, status,
                DateTime.Now, acceptanceDate))
        {
            DialogResult = true;
            Close();
        }
    }
}
