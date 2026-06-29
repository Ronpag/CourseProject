using System.Windows;
using System.Windows.Controls;
using CRM.Data;

namespace CRM.View;

public partial class TaskListPage : Page
{
    public TaskListPage()
    {
        InitializeComponent();
        LoadTasks();
    }

    private void RegisterBtn(object sender, RoutedEventArgs e)
    {
        var window = new RegisterTaskWindow();

        if (window.ShowDialog() == true)
        {
            LoadTasks();

            MessageBox.Show(
                "Task created",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
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