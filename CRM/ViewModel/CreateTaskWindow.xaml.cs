using System.Windows;
using CRM.Data;

namespace CRM.ViewModel.UserWindow;

public partial class CreateTaskWindow : Window
{
    private readonly int _workerId;

    public CreateTaskWindow(int workerId)
    {
        InitializeComponent();

        _workerId = workerId;

        LoadClients();
    }

    private void LoadClients()
    {
        using var db = new AppDbContext();

        ClientsBox.ItemsSource = db.Clients.ToList();
    }

    private void CreateBtn(object sender, RoutedEventArgs e)
    {
        string taskName = TaskNameBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(taskName))
        {
            MessageBox.Show("Enter task name");
            return;
        }

        if (ClientsBox.SelectedItem is not Client selectedClient)
        {
            MessageBox.Show("Select client");
            return;
        }

        using var db = new AppDbContext();

        var client = db.Clients.FirstOrDefault(c => c.Id == selectedClient.Id);

        if (client == null)
        {
            MessageBox.Show("Client not found");
            return;
        }

        db.Tasks.Add(new CRM.Data.Task
        {
            TaskName = taskName,
            Description = DescriptionBox.Text.Trim(),
            ClientId = client.Id,
            WorkerId = _workerId,
            Status = CRM.Data.Task.TaskStatus.Assigned,
            StartDate = DateTime.Now,
            AcceptanceDate = DateTime.Now
        });

        client.CountOrders++;

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}
