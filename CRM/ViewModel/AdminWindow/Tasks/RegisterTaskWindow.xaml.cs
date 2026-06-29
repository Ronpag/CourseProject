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

        if (ClientsBox.SelectedItem is not Client client)
        {
            MessageBox.Show(
                "Select client",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (WorkersBox.SelectedItem is not User worker)
        {
            MessageBox.Show(
                "Select worker",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        using var db = new AppDbContext();

        var selectedClient = db.Clients.FirstOrDefault(c => c.Id == client.Id);

        if (selectedClient == null)
        {
            MessageBox.Show("Client not found");
            return;
        }

        db.Tasks.Add(new CRM.Data.Task
        {
            TaskName = taskName,
            ClientId = selectedClient.Id,
            WorkerId = worker.Id,
            Status = CRM.Data.Task.TaskStatus.Assigned
        });

        selectedClient.CountOrders++;

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}