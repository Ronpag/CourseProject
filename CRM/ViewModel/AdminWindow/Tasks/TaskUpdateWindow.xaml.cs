using System.Windows;
using CRM.Data;

namespace CRM.View;

public partial class TaskUpdateWindow : Window
{
    private readonly int _taskId;

    public TaskUpdateWindow(CRM.Data.Task task)
    {
        InitializeComponent();

        _taskId = task.Id;

        TaskNameBox.Text = task.TaskName;
        ClientIdBox.Text = task.ClientId.ToString();
        WorkerIdBox.Text = task.WorkerId.ToString();

        StatusBox.ItemsSource =
            Enum.GetValues(typeof(CRM.Data.Task.TaskStatus));

        StatusBox.SelectedItem = task.Status;
    }

    private void SaveChangesBtn(object sender, RoutedEventArgs e)
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

        if (!int.TryParse(ClientIdBox.Text, out int clientId))
        {
            MessageBox.Show(
                "Invalid Client Id",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        var status = (CRM.Data.Task.TaskStatus)StatusBox.SelectedItem;

        int? workerId = null;

        if (status != CRM.Data.Task.TaskStatus.Available)
        {
            if (!int.TryParse(WorkerIdBox.Text, out int parsedWorkerId))
            {
                MessageBox.Show(
                    "Invalid Worker Id",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            workerId = parsedWorkerId;
        }

        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == _taskId);

        if (task == null)
        {
            MessageBox.Show(
                "Task not found",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            return;
        }

        var client = db.Clients.FirstOrDefault(c => c.Id == clientId);

        if (client == null)
        {
            MessageBox.Show(
                "Client with this ID does not exist",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        if (workerId != null)
        {
            var worker = db.Users.FirstOrDefault(u => u.Id == workerId);

            if (worker == null)
            {
                MessageBox.Show(
                    "Worker with this ID does not exist",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }
        }

        if (task.ClientId != clientId)
        {
            var oldClient = db.Clients.FirstOrDefault(c => c.Id == task.ClientId);

            if (oldClient != null && oldClient.CountOrders > 0)
            {
                oldClient.CountOrders--;
            }

            client.CountOrders++;
        }

        task.TaskName = taskName;
        task.ClientId = clientId;
        task.WorkerId = workerId;
        task.Status = status;

        db.SaveChanges();

        DialogResult = true;
        Close();
    }
}