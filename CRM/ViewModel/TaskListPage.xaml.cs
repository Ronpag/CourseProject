using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CRM.Data;
using CRM.View;

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
        if (TasksList == null) return;

        using var db = new AppDbContext();

        var query = db.Tasks.AsQueryable();

        var statusFilters = new List<CRM.Data.Task.TaskStatus>();
        if (ChkPending.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Pending);
        if (ChkAvailable.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Available);
        if (ChkAssigned.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Assigned);
        if (ChkInProgress.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.InProgress);
        if (ChkCompleted.IsChecked == true) statusFilters.Add(CRM.Data.Task.TaskStatus.Completed);

        if (statusFilters.Count > 0)
            query = query.Where(t => statusFilters.Contains(t.Status));

        if (DateFrom?.SelectedDate != null)
            query = query.Where(t => t.StartDate >= DateFrom.SelectedDate.Value);

        if (DateTo?.SelectedDate != null)
            query = query.Where(t => t.StartDate <= DateTo.SelectedDate.Value);

        TasksList.ItemsSource = query.ToList();
    }

    private void FilterChanged(object sender, RoutedEventArgs e)
    {
        LoadTasks();
    }

    private void DateFilterChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        LoadTasks();
    }

    private void DetailsBtn(object sender, RoutedEventArgs e)
    {
        if (TasksList.SelectedItem is not CRM.Data.Task task) return;
        new DetailsWindow(task).ShowDialog();
    }

    private void TasksList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (TasksList.SelectedItem is not CRM.Data.Task task) return;
        new DetailsWindow(task).ShowDialog();
    }
}