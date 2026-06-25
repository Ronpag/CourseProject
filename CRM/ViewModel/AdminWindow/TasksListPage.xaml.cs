using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.View;

public partial class TasksListPage : Page
{
    public TasksListPage()
    {
        InitializeComponent();
        LoadTasks();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        string taskName = TaskName.Text?.Trim() ?? "";

        if (!int.TryParse(ClientId.Text, out int clientId))
        {
            MessageBox.Show("Invalid ClientId");
            return;
        }

        if (!int.TryParse(WorkerId.Text, out int workerId))
        {
            MessageBox.Show("Invalid WorkerId");
            return;
        }

        if (string.IsNullOrWhiteSpace(taskName))
        {
            MessageBox.Show(
                "Task name is empty",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        using var db = new AppDbContext();

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

        var task = new CRM.Data.Task
        {
            TaskName = taskName,
            ClientId = clientId,
            WorkerId = workerId,
            Status = CRM.Data.Task.TaskStatus.Assigned
        };

        client.CountOrders++;

        db.Tasks.Add(task);
        db.SaveChanges();

        LoadTasks();

        MessageBox.Show(
            "Task created",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void DeleteBtn(object sender, RoutedEventArgs e)
    {
        if (TasksList.SelectedItem is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show("Select task");
            return;
        }

        using var db = new AppDbContext();

        var task = db.Tasks.FirstOrDefault(t => t.Id == selectedTask.Id);

        if (task == null)
            return;

        var client = db.Clients.FirstOrDefault(c => c.Id == task.ClientId);

        if (client != null && client.CountOrders > 0)
        {
            client.CountOrders--;
        }

        db.Tasks.Remove(task);
        db.SaveChanges();

        LoadTasks();

        MessageBox.Show(
            "Task deleted",
            "Success",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void UpdateBtn(object sender, RoutedEventArgs e)
    {
        if (TasksList.SelectedItem is not CRM.Data.Task selectedTask)
        {
            MessageBox.Show(
                "Select task",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        var window = new TaskUpdateWindow(selectedTask);

        bool? result = window.ShowDialog();

        if (result == true)
        {
            LoadTasks();

            MessageBox.Show(
                "Task updated",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    private void LoadTasks()
    {
        using var db = new AppDbContext();

        TasksList.ItemsSource = db.Tasks.ToList();
    }
}