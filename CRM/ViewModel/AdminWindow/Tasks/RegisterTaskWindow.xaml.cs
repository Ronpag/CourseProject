using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class RegisterTaskWindow : Window
{
    public RegisterTaskWindow()
    {
        InitializeComponent();

        LoadClients();
        LoadWorkers();
    }

    private void LoadClients()
    {
        using var db = new AppDbContext();

        ClientsBox.ItemsSource = db.Clients.ToList();
    }

    private void LoadWorkers()
    {
        using var db = new AppDbContext();

        WorkersBox.ItemsSource = db.Users
            .Where(u => !u.IsAdmin)
            .ToList();
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
            MessageBox.Show(
                "Task name is empty",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (ClientsBox.SelectedItem is not Client selectedClient)
        {
            MessageBox.Show(
                "Select client",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        using var db = new AppDbContext();

        var client = db.Clients.FirstOrDefault(c => c.Id == selectedClient.Id);

        if (client == null)
        {
            MessageBox.Show(
                "Client not found",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            return;
        }

        int? workerId = null;
        CRM.Data.Task.TaskStatus status = CRM.Data.Task.TaskStatus.Available;

        if (AssignWorkerCheckBox.IsChecked == true)
        {
            if (WorkersBox.SelectedItem is not User selectedWorker)
            {
                MessageBox.Show(
                    "Select worker",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            workerId = selectedWorker.Id;
            status = CRM.Data.Task.TaskStatus.Assigned;
        }

        db.Tasks.Add(new CRM.Data.Task
        {
            TaskName = taskName,
            ClientId = client.Id,
            WorkerId = workerId,
            Status = status
        });

        client.CountOrders++;

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}